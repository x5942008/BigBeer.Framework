using BigBeer.Freamework.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BigBeer.Test.Service
{
    /// <summary>
    /// 测试服务
    /// </summary>
    public class BigBeerTestService : BaseService
    {
        public override string Display => "大雄框架测试服务";

        public override string Name => "BigBeerTestService";

        string path => @"E:\Log\";

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="msg"></param>
        void Logger(string msg)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                var filePath = $"{path}\\{DateTime.Now.Date.ToString("yyMMdd")}.{Name}.txt";
                var message = new StringBuilder();
                message.AppendLine($"-->{DateTime.Now.ToString()}");
                message.AppendLine(msg);
                using (var stream = File.OpenWrite(filePath))
                {
                    var buffer = UTF8Encoding.UTF8.GetBytes(message.ToString());
                    stream.Position = stream.Length;
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }


        public override void Start()
        {
            Logger( JsonConvert.SerializeObject(new {a = 1,b =DateTime.Now.ToString()}));
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
