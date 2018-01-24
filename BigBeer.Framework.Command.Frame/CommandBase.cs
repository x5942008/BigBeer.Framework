using BigBeer.Framework.Command.Frame.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.Command.Frame
{
    public abstract class CommandBase : MarshalByRefObject, ICommand
    {
        public virtual string UniqueId => Guid.NewGuid().ToString();

        public abstract string Display { get; }

        public abstract string Command { get; }

        protected List<string> Result { get; } = new List<string>();

        /// <summary>
        /// 新增输出消息
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnMessage(string message)
        {
            Result.Add($"{DateTime.Now.ToString("HH:mm:ss")} {message}");
        }

        public virtual string[] Execute(string command, string[] parameters = null)
        {
            return Result.ToArray();
        }
    }
}
