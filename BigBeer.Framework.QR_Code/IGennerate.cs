using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.QR_Code
{
    /// <summary>
    /// 二维码生成
    /// </summary>
    public interface IGennerate
    {
        void CreateQRcode(string url,string path = null);
    }
}
