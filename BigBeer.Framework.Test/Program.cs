using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BigBeer.AI.Voice;
using BigBeer.Framework.QR_Code;
using System.Security.Cryptography;

namespace BigBeer.Framework.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 语音生成
            //string content = "大扎好，我系轱天乐，我四脏渣辉，探挽懒月，介四里没有挽过的船新版本，挤需体验三番钟，里造会干我一样，爱象戒宽油系";
            //string vpath = @"E:\Voice\";
            //string filename = Guid.NewGuid().ToString();
            //VoiceType vtype = VoiceType.mp3;
            //int per = 1;
            //Voices.Tts(content,vpath,filename,vtype,per);
            //Console.WriteLine($"生成文件{filename}.{vtype}成功,文件保存路径:{vpath}");
            //Console.WriteLine("继续生成二维码条形码请按回车键！");
            //Console.ReadLine();
            #endregion

            #region 二维码条形码生成
            //string ipath = @"E:\Image\";
            //string url = "http://www.wujixiong.com";
            //string name1 = "wujixiong";
            //string name2 = "bigbeerqr";
            //string name3 = "bigbeerzx";
            //string BarCode = "20180123";//条形码数字（双数，80个以内）
            //ImgType itype = ImgType.Png;
            //var sufiix = ImgSuffix.Getsuffix(itype);
            //string logo = @"C:\Users\wjx\Desktop\wujixiong.jpg";//Logo地址 需要修改
            //if (Directory.Exists(logo)) Console.WriteLine("Logo地址不正确,请重新确认地址");
            //QrCode_QR.Generate(url, name1, ipath, itype);//普通二维码
            //Console.WriteLine($"文件保存成功，请在路径{ipath}查看文件{name1}{sufiix}");
            //QRCode_Zxing.Generate(url, logo, ipath, name2, itype, 800, 800);//带logo二维码 并且自定义大小
            //Console.WriteLine($"文件保存成功，请在路径{ipath}查看文件{name2}{sufiix}");
            //QRCode_Zxing.Generate2(BarCode, ipath, name3, itype, 300, 100);//宽高比例3:1
            //Console.WriteLine($"文件保存成功，请在路径{ipath}查看文件{name3}{sufiix}");
            ////读取解析二维码和条形码
            //var result1 = QRCode_Zxing.Read($"{ipath}{name1}{sufiix}");
            //var result2 = QRCode_Zxing.Read($"{ipath}{name2}{sufiix}");
            //var result3 = QRCode_Zxing.Read($"{ipath}{name3}{sufiix}");
            //Console.WriteLine(result1);
            //Console.WriteLine(result2);
            //Console.WriteLine(result3);
            #endregion

            var a = "01B61C";
            var result = Convert.ToInt32(a,16);
            var b = 11111;
            var data = Convert.ToString(b,2);
            var data1 = Convert.ToString(b,8);
            var data2 = Convert.ToString(b,10);
            var data3 = Convert.ToString(b,16);
            var c = 0 + data3;
            Console.WriteLine(Convert.ToInt32(c, 16));
            Console.WriteLine(c);
            //Console.WriteLine(data1);
            //Console.WriteLine(data2);
            Console.WriteLine(data3);
            //int temp = 55;
            //var sh = new SHA1Managed();
            //byte[] data = sh.ComputeHash(Encoding.UTF8.GetBytes(temp.ToString()));
            //var result = Convert.ToBase64String(data,Base64FormattingOptions.None);
            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
