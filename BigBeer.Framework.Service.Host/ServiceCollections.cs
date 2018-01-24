using BigBeer.Freamework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.Service.Host
{
    public class ServiceCollections
    {
        public ServiceCollections() {
            dictionary = new Dictionary<string, Type>();
            Service = new List<IService>();
        }
        private Dictionary<string, Type> dictionary { get; set; }
        private IList<IService> Service { get; set; }
        public string[] Start(){
            var result = new List<string>();
            foreach (var service in dictionary)
            {
                try
                {
                    var activitor =(IService)Activator.CreateInstance(service.Value);
                    activitor.Start();
                    result.Add($"{service.Key}服务启动成功");
                    Service.Add(activitor);
                }
                catch (Exception ex)
                {
                    result.Add($"{service.Key}服务启动失败 ex:{ex.Message}");
                }
            }
            return result.ToArray();
        }
        public string[] Stop(){
            var result = new List<string>();
            for (int i = 0; i < Service.Count(); i++)
            {
                var key = Service[i];
                try
                {
                    key.Stop();
                    result.Add($"{key.Name}服务停止成功");
                    Service.Remove(key);
                }
                catch (Exception ex)
                {
                    result.Add($"{key.Name}服务停止错误：{ex.Message}");
                }
                finally {

                }
            }
            return result.ToArray();
        }
        public ServiceCollections Add(string name, Type serviceType)
        {
            if (dictionary.ContainsKey(name))
                return this;
            dictionary.Add(name, serviceType);
            return this;
        }
        public ServiceCollections Add<T>(string name) where T : IService
        {
            if (dictionary.ContainsKey(name))
                return this;
            dictionary.Add(name, typeof(T));
            return this;
        }

        public ServiceCollections Add(Type serviceType)
        {
            return Add(serviceType.Name, serviceType);
        }

        public ServiceCollections Add<T>() where T : IService
        {
            return Add<T>(typeof(T).Name);
        }
    }
}
