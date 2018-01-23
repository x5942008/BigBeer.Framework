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
        /// <summary>
        /// 生成中文二维码-支持中文
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filename"></param>
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

        //生成中文二维码-支持中文(自定义大小)
        public static void Generate(string url, string filename, ImgType type =  ImgType.Png, int? width =null,int? high =null)
        {
            QrEncoder qrEncoder = new QrEncoder();
            var qrCode = qrEncoder.Encode(url);
            //保存成png文件
            //string filename = @"H:\桌面\截图\cn.png";
            string suffix;//后缀
            switch (type)
            {
                case ImgType.MemoryBmp:
                    suffix = ".memoryBmp";
                    break;
                case ImgType.Bmp:
                    suffix = ".bmp";
                    break;
                case ImgType.Emf:
                    suffix = ".emf";
                    break;
                case ImgType.Wmf:
                    suffix = ".wmf";
                    break;
                case ImgType.Gif:
                    suffix = ".gif";
                    break;
                case ImgType.Jpeg:
                    suffix = ".jpeg";
                    break;
                case ImgType.Png:
                    suffix = ".png";
                    break;
                case ImgType.Tiff:
                    suffix = ".tiff";
                    break;
                case ImgType.Exif:
                    suffix = ".exif";
                    break;
                case ImgType.Icon:
                    suffix = ".icon";
                    break;
                default:
                    suffix = ".png";
                    break;
            }
            filename = $"{filename}{suffix}";
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
    public enum ImgType
    {
        // 在内存中的位图的格式。
        MemoryBmp,
        //位图 (BMP) 图像格式
        Bmp,
        //增强型图元文件 (EMF) 图像格式
        Emf,
        //Windows 图元文件 (WMF) 映像格式
        Wmf,
        //图形交换格式 (GIF) 图像格式。
        Gif,
        // 联合图像专家组 (JPEG) 图像格式。
        Jpeg,
        // W3C 可移植网络图形 (PNG) 图像格式。
        Png,
        // 图像文件格式 (TIFF) 图像格式。
        Tiff,
        // 获取可交换图像文件 (Exif) 格式。
        Exif,
        //Windows 图标图像格式。
        Icon
    }
}
