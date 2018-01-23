using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigBeer.Framework.QR_Code;

namespace BigBeer.Framework.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "";
            QrCode_QR.Generate("http://www.wujixiong.com","bigbeerqr", ImgType.Png,500,500);
            QrCode_QR.GenerateNomarl("http://www.wujixiong.com", "bigbeerqr");
            QRCode_Zxing.Generate();
            Console.WriteLine($"文件已保存，请在路径{path}查看");
            Console.ReadLine();
        }
    }
}
