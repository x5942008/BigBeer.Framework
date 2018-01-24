using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Freamework.Service.Interface
{
    /// <summary>
    /// MarshalByRefObject 允许在支持远程处理的应用程序中跨应用程序域边界访问对象。
    /// </summary>
    public abstract class BaseService : MarshalByRefObject, IService
    {
        public virtual string UniqueId => Guid.NewGuid().ToString();
        public abstract string Display { get; }
        public abstract string Name { get; }

        protected List<string> Result { get; } = new List<string>();

        public abstract void Start();
        public abstract void Stop();

        /// <summary>
        /// 重写继承类重置时间
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            ILease alease = (ILease)base.InitializeLifetimeService();
            if (alease != null)
            {
                alease.InitialLeaseTime = TimeSpan.Zero;
                return alease;
            }
            return base.InitializeLifetimeService();
        }

    }
}
