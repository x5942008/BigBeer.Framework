using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Framework
{
    /// <summary>
    /// 配置
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 服务描述
        /// </summary>
        string Discription { get; set; }
        /// <summary>
        /// 日志路径
        /// </summary>
        string LogoPath { get; set; }
        /// <summary>
        /// 数据字典
        /// </summary>
        IDictionary<string, string> Dictionary { get; set; }
    }
}
