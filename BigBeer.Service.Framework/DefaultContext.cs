using BigBeer.Service.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service
{
    /// <summary>
    /// 默认上下文对象
    /// </summary>
    [Serializable]
    public class DefaultContext : MarshalByRefObject, IContext
    {
        /// <summary>
        /// 上下文
        /// </summary>
        public IDictionary<string, string> Context { get; set; } = new Dictionary<string, string>();


        public string AppDomanName { get; set; }

        public string ComputerName { get; set; } = Environment.MachineName;
    }
}
