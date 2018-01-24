using BigBeer.Service.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigBeer.Service.Host.App
{
    public class Service
    {
        /// <summary>
        /// 服务程序唯一编号
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();
        /// <summary>
        /// 程序名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// appdoman名称
        /// </summary>
        public string AppDomanName { get; set; }
        /// <summary>
        /// 类名称
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// 程序集名
        /// 命名空间.类
        /// </summary>
        public string TypeName { get; set; }


        private IApp _app;
        private Timer hartTimer { get; set; }
        /// <summary>
        /// 当前服务
        /// </summary>
        public IApp App
        {
            get
            {
                return _app;
            }
            set
            {
                _app = value;
                //更新服务状态和心跳跟踪
                hartTimer = new Timer(new TimerCallback((o) => {
                    if (Status == AppStatus.Stop) return;
                    if (Status == AppStatus.Stop)
                        hartTimer.Dispose();
                    try
                    {
                        //_app.Hart();
                        Status = _app.RunStatus.Status;
                        //Program.Logger("", $"{DateTime.Now.ToString("HH:mm:ss")} 心跳成功.{this.AppDomanName}:{Status.ToString()}");
                    }
                    catch (Exception ex)
                    {
                        Status = AppStatus.Error;
                        Program.Logger("", $"{DateTime.Now.ToString("HH:mm:ss")} 心跳错误.{this.AppDomanName}:{ex.Message}");
                    }
                }), null, 30 * 1000, 60 * 1000);
                //hartTimer.Change(0, 30 * 1000);
            }
        }
        /// <summary>
        /// 当前的appdomain
        /// </summary>
        public AppDomain AppDomain { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        public AppStatus Status { get; set; } = AppStatus.Preparing;
    }
}
