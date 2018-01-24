using BigBeer.Service.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Service.Host.App
{
    /// <summary>
    /// 服务配置文件
    /// </summary>
    public class ServiceConfig
    {
        /// <summary>
        /// 日志路径
        /// </summary>
        public string LogPath { get; set; } = $"{ AppDomain.CurrentDomain.BaseDirectory}\\log\\";
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<ServiceConfig> Load(string filePath = null)
        {
            if (filePath == null)
                filePath = $"{ AppDomain.CurrentDomain.BaseDirectory}\\Config.json";
            ServiceConfig result = null;
            try
            {
                var configjson = await new StreamReader(filePath).ReadToEndAsync();
                return configjson.ToObject<ServiceConfig>();
            }
            catch (Exception)
            {
                return result;
            }
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<bool> Save(string filePath = null)
        {
            if (filePath == null)
                filePath = $"{ AppDomain.CurrentDomain.BaseDirectory}\\Config.json";
            try
            {
                var configjson = this.ToJson();
                await new StreamWriter(filePath, false).WriteAsync(configjson);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
