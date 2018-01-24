using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Framework
{
    public abstract class AppBase : MarshalByRefObject, IApp
    {
        public AppBase()
        {
            RunStatus.Status = AppStatus.Preparing;
            OnMessage("准备中..");
        }
        #region appBase
        /// <summary>
        /// 当前上下文
        /// </summary>
        protected IContext Context { get; set; }

        /// <summary>
        /// 当前程序配置
        /// </summary>
        protected IConfig Config { get { return LoadConfig(); } }
        /// <summary>
        /// 当前状态
        /// </summary>
        public IRunStatus RunStatus { get; } = new RunStatus();
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual IConfig LoadConfig(string path = null)
        {
            if (path == null)
                path = $"{AppDomain.CurrentDomain.BaseDirectory}\\Config.json";
            IConfig result = null;
            try
            {
                var configjson = new FileInfo(path).OpenText().ReadToEnd();
                return configjson.ToObject<Config>();
            }
            catch (Exception ex)
            {
                return result;
            }
        }


        /// <summary>
        /// 设置当前状态
        /// </summary>
        /// <param name="status"></param>
        protected virtual void ChangeStatus(AppStatus status = AppStatus.Preparing)
        {
            RunStatus.Status = status;
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual async Task Log(string message, int tryNumber = 0)
        {
            if (tryNumber > 10)
            {
                RunStatus.Message.Add(DateTime.Now, $"log error :out of number [{tryNumber}]");
                return;
            }
            try
            {
                if (!Directory.Exists(Config.LogoPath))
                    Directory.CreateDirectory(Config.LogoPath);
                using (StreamWriter sw = new StreamWriter(Path.Combine(Config.LogoPath, $"{DateTime.Now.ToString("yyyyMMdd")}.log"), true))
                {
                    await sw.WriteLineAsync(DateTime.Now.ToString());
                    await sw.WriteLineAsync(message);
                    sw.Flush();
                }
            }
            catch (Exception ex)
            {
                await Task.Delay(100);
                await Log(message, tryNumber++);
            }
        }
        /// <summary>
        /// 发生错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task OnError(string message)
        {
            ChangeStatus(AppStatus.Error);
            if (RunStatus.Error.Count() > 10)
                RunStatus.Error.Clear();
            try
            {
                RunStatus.Error.Add(DateTime.Now, message);
                await Log(message);
            }
            catch (Exception)
            {
                await Task.Delay(100);
                await OnError(message);
            }


        }
        /// <summary>
        /// 记录消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual async Task OnMessage(string message)
        {
            if (RunStatus.Message.Count() > 10)
                RunStatus.Message.Clear();
            try
            {
                RunStatus.Message.Add(DateTime.Now, message);
                await Log(message);
            }
            catch (Exception)
            {
                await Task.Delay(100);
                await OnMessage(message);
            }
        }
        #endregion
        #region IApp
        public virtual async void Hart()
        {
            await Task.Delay(100);
        }
        public virtual bool Stop()
        {
            RunStatus.Status = AppStatus.Stop;
            return true;
        }
        public virtual async void Run(IContext context)
        {
            Context = context;
            RunStatus.StartTime = DateTime.Now;

            await OnMessage("启动中..");
        }
        public virtual bool ReStart()
        {
            return true;
        }
        #endregion
    }
}
