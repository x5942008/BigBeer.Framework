using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Framework
{
    /// <summary>
    /// 运行状态
    /// </summary>
    public interface IRunStatus
    {
        /// <summary>
        /// 启动时间
        /// </summary>
        DateTime StartTime { get; set; }
        /// <summary>
        /// 当前状态
        /// </summary>
        AppStatus Status { get; set; }
        /// <summary>
        /// 错误日志
        /// </summary>
        IDictionary<DateTime, string> Error { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        IDictionary<DateTime, string> Message { get; set; }


    }
}
