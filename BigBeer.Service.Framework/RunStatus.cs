using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Framework
{
    [Serializable]
    public class RunStatus : MarshalByRefObject, IRunStatus
    {
        public DateTime StartTime { get; set; }
        public AppStatus Status { get; set; }
        public IDictionary<DateTime, string> Error { get; set; } = new Dictionary<DateTime, string>();
        public IDictionary<DateTime, string> Message { get; set; } = new Dictionary<DateTime, string>();
    }
}
