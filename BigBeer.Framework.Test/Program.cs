using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
            string path = @"F:\";
            string url = "http://www.wujixiong.com";
            string name1 = "wujixiong";
            string name2 = "bigbeerqr";
            string name3 = "bigbeerzx";
            string BarCode = "20180123";//条形码数字（双数，80个以内）
            ImgType type = ImgType.Png;
            var sufiix = Suffix.Getsuffix(type);
            string logo = @"F:\123.jpg";//Logo地址 需要修改
            QrCode_QR.Generate(url,name1,path,type);//普通二维码
            Console.WriteLine($"文件保存成功，请在路径{path}查看文件{name1}{sufiix}");
            QRCode_Zxing.Generate(url,logo,path,name2,type,800,800);//带logo二维码 并且自定义大小
            Console.WriteLine($"文件保存成功，请在路径{path}查看文件{name2}{sufiix}");
            QRCode_Zxing.Generate2(BarCode,path,name3,type);
            Console.WriteLine($"文件保存成功，请在路径{path}查看文件{name3}{sufiix}");
            //读取解析二维码和条形码
            var result1 =  QRCode_Zxing.Read($"{path}{name1}{sufiix}");
            var result2 = QRCode_Zxing.Read($"{path}{name2}{sufiix}");
            var result3 = QRCode_Zxing.Read($"{path}{name3}{sufiix}");
            Console.WriteLine(result1);
            Console.WriteLine(result2);
            Console.WriteLine(result3);

            Console.ReadLine();
        }
    }
}
