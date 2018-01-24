using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BigBeer.Service.Framework
{
    public static partial class Extensions
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static string Serialize(object value)
        {
            try
            {
                return JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            }
            catch
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        static T Deserialize<T>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);

            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        /// <summary>
        /// 对象序列化为json
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(this object value)
        {
            if (value == null) return string.Empty;
            try
            {
                return Serialize(value);
            }
            catch
            {
                return string.Empty;
            }

        }
        /// <summary>
        /// 序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string @this)
        {
            if (string.IsNullOrEmpty(@this)) return default(T);
            return Deserialize<T>(@this);
        }
    }
}
