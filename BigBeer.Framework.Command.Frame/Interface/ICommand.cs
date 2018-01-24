using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.Command.Frame.Interface
{
    public interface ICommand
    {
        /// <summary>
        /// 唯一id
        /// </summary>
        string UniqueId { get; }

        /// <summary>
        /// 说明
        /// </summary>
        string Display { get; }
        /// <summary>
        /// 触发excute的命令
        /// </summary>
        string Command { get; }
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">当前输入的命令</param>
        /// <param name="parameters">参数</param>
        /// <returns>命令执行结果</returns>
        string[] Execute(string command, string[] parameters = null);
    }
}
