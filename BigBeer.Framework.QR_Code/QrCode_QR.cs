using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.QR_Code
{
    class QrCode_QR
    {
        #region 引用Nuget包 QrCode.Net 
        public static void GenerateNomarl(string url, string filename)
        {
            QrEncoder qrEncoder = new QrEncoder();
            var qrCode = qrEncoder.Encode(url);
            //保存成png文件
            //string filename = @"H:\桌面\截图\url.png";
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
            }
        }

        //生成中文二维码-支持中文
       public static void Generate(string url,string filename,int? width =null,int? high =null)
        {
            QrEncoder qrEncoder = new QrEncoder();
            var qrCode = qrEncoder.Encode(url);
            //保存成png文件
            //string filename = @"H:\桌面\截图\cn.png";
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            int w = 500;
            int h = 500;
            if (width!=null) w = (int)width;
            if (high != null) h = (int)high;
            Bitmap map = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(map);
            g.FillRectangle(Brushes.Red, 0, 0, 500, 500);
            render.Draw(g, qrCode.Matrix, new Point(20, 20));
            map.Save(filename, ImageFormat.Png);
        }

        //设置二维码大小
        static void Generate4()
        {
            QrEncoder qrEncoder = new QrEncoder();
            var qrCode = qrEncoder.Encode("我是小天马");
            //保存成png文件
            string filename = @"H:\桌面\截图\size.png";
            //ModuleSize 设置图片大小  
            //QuietZoneModules 设置周边padding
            /*
                * 5----150*150    padding:5
                * 10----300*300   padding:10
                */
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(10, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            Point padding = new Point(10, 10);
            DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
            Bitmap map = new Bitmap(dSize.CodeWidth + padding.X, dSize.CodeWidth + padding.Y);
            Graphics g = Graphics.FromImage(map);
            render.Draw(g, qrCode.Matrix, padding);
            map.Save(filename, ImageFormat.Png);
        }

        //生成带Logo的二维码
        static void Generate5()
        {
            QrEncoder qrEncoder = new QrEncoder();
            var qrCode = qrEncoder.Encode("我是小天马");
            //保存成png文件
            string filename = @"H:\桌面\截图\logo.png";
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            DrawingSize dSize = render.SizeCalculator.GetSize(qrCode.Matrix.Width);
            Bitmap map = new Bitmap(dSize.CodeWidth, dSize.CodeWidth);
            Graphics g = Graphics.FromImage(map);
            render.Draw(g, qrCode.Matrix);
            //追加Logo图片 ,注意控制Logo图片大小和二维码大小的比例
            Image img = Image.FromFile(@"F:\JavaScript_Solution\QrCode\QrCode\Images\101.jpg");

            Point imgPoint = new Point((map.Width - img.Width) / 2, (map.Height - img.Height) / 2);
            g.DrawImage(img, imgPoint.X, imgPoint.Y, img.Width, img.Height);
            map.Save(filename, ImageFormat.Png);
        }
        #endregion
    }
}
