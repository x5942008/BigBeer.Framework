using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Framework
{
    public interface IApp
    {
        /// <summary>
        /// 重新开始
        /// </summary>
        /// <returns></returns>
        bool ReStart();
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="context"></param>
        void Run(IContext context);
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        bool Stop();
        /// <summary>
        /// 心跳每4.9分钟一次
        /// 避免服务被注销
        /// </summary>
        void Hart();
        /// <summary>
        /// 运行状态
        /// </summary>
        /// <returns></returns>
        IRunStatus RunStatus { get; }
    }
}
