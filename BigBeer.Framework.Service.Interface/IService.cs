using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Freamework.Service.Interface
{
    public interface IService
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
        /// 名字
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 启动服务
        /// </summary>
        void Start();
        /// <summary>
        /// 停止服务
        /// </summary>
        void Stop();
    }
}
