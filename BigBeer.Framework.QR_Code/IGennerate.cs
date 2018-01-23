using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.QR_Code
{
    /// <summary>
    /// 二维码生成接口 未使用继承
    /// </summary>
    public interface IGennerate
    {
        /// <summary>
        /// 生成默认二维码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        void GenerateNomarl(string url, string path = null);
        /// <summary>
        /// 自定义二维码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        void Generate(string url,string path = null,int?w=null,int?h =null);
    }
}
