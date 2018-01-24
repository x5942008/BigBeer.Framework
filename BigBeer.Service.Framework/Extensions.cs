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
        public static string ToJson(this object value)
        {
            return "";
        }

        public static object ToObject<T>(this string value)
        {
            return null;
        }
    }
}
