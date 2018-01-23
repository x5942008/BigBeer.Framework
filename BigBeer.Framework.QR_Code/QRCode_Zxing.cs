using Gma.QrCodeNet.Encoding;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace BigBeer.Framework.QR_Code
{
    public class QRCode_Zxing
    {
        #region C#生成二维码-基础版本
        //需要引用ZXing.DLL  可以百度一下  一个很好用的二维码生成程序集《DLL文件啦》
        //使用案例：Bitmap img3 = GenByZXingNet("");
        //使用案例：img3.Save(Server.MapPath(@"\testImg\erweima.png"));
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">二维码信息</param>
        /// <param name="codeSizeInPixels">正方形 边长</param>
        /// <returns>图片</returns>
        static Bitmap GenByZXingNet(string msg, int codeSizeInPixels = 250)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");//编码问题
            writer.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);

            writer.Options.Height = writer.Options.Width = codeSizeInPixels;
            writer.Options.Margin = 0;//设置边框1            ZXing.Common.BitMatrix bm = writer.Encode(msg);
            Bitmap img = writer.Write(msg);
            return img;
        }
        #endregion

        #region C#生成带有Log的二维码
        /// <summary>
        /// 生成带Logo的二维码
        /// </summary>
        /// <param name="text">文本内容</param>
        public static void Generate(string text, string LogoPth, string serverPth,string filename,ImgType type, /* ImageFormat imgFrt,*/int? width=null,int? hight =null)
        {
            try
            {
                //Logo 图片
                Bitmap logo = new Bitmap(LogoPth);
                //构造二维码写码器
                MultiFormatWriter writer = new MultiFormatWriter();
                Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
                hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
                hint.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);

                int w = 300;
                int h = 300;
                if (width != null) w = (int)width;
                if (hight != null) h = (int)hight;
                //生成二维码 
                ZXing.Common.BitMatrix bm = writer.encode(text, BarcodeFormat.QR_CODE, w, h, hint);
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                Bitmap map = barcodeWriter.Write(bm);


                //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
                int[] rectangle = bm.getEnclosingRectangle();

                //计算插入图片的大小和位置
                int middleW = Math.Min((int)(rectangle[2] / 3.5), logo.Width);
                int middleH = Math.Min((int)(rectangle[3] / 3.5), logo.Height);
                int middleL = (map.Width - middleW) / 2;
                int middleT = (map.Height - middleH) / 2;

                //将img转换成bmp格式，否则后面无法创建Graphics对象
                Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmpimg))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.DrawImage(map, 0, 0);
                }
                //将二维码插入图片
                Graphics myGraphic = Graphics.FromImage(bmpimg);
                //白底
                myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
                myGraphic.DrawImage(logo, middleL, middleT, middleW, middleH);
                string suffix = Suffix.Getsuffix(type); ;//后缀
                //保存成图片
                bmpimg.Save($"{serverPth}{filename}{suffix}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
        }
        #endregion

        #region ZXing生成条形码
        /// </summary>
        /// 文本内容
        /// <param name="text">只支持数字 只支持偶数个 最大长度80</param>
        /// <param name="serverPth">存储路径 @"H:\桌面\截图\"</param>
        /// <param name="filename">文件名不带后缀</param>
        /// <param name="type">文件格式</param>
        /// <param name="width">宽度比例3</param>
        /// <param name="hight">高度比例1</param>
        public static void Generate2(string text, string serverPth, string filename, ImgType type, /*ImageFormat imgFrt,*/int? width = null, int? hight = null)
        {
            BarcodeWriter writer = new BarcodeWriter();
            //使用ITF 格式，不能被现在常用的支付宝、微信扫出来
            //如果想生成可识别的可以使用 CODE_128 格式
            //writer.Format = BarcodeFormat.ITF;
            writer.Format = BarcodeFormat.CODE_128;
            int w = 150;
            int h = 50;
            if (width != null) w = (int)width;
            if (hight != null) h = (int)hight;
            EncodingOptions options = new EncodingOptions()
            {
                Width = w,
                Height = h,
                Margin = 2
            };
            writer.Options = options;
            Bitmap map = writer.Write(text);
            string suffix = Suffix.Getsuffix(type); ;//后缀
            filename =$"{serverPth}{filename}{suffix}";//请注意 保存格式和文件扩展名一致性
            map.Save(filename);
        }
        #endregion

        #region 二维码 条形码读取
        /// <summary>
        /// 二维码 条形码读取
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static string Read(string filename)
        {
            BarcodeReader reader = new BarcodeReader();
            reader.Options.CharacterSet = "UTF-8";
            Bitmap map = new Bitmap(filename);
            Result result = reader.Decode(map);
            return result == null ? "" : result.Text;
        }
        #endregion

        #region C#生成特定颜色二维码-基础版本
        //需要引用ZXing.DLL  可以百度一下  一个很好用的二维码生成程序集《DLL文件啦》
        //使用案例：Bitmap img3 = GenByZXingNet("");
        //使用案例：img3.Save(Server.MapPath(@"\testImg\erweima.png"));
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">二维码信息</param>
        /// <param name="codeSizeInPixels">正方形 边长</param>
        /// <returns>图片</returns>
        static Bitmap GenByZXingNet_Color(string msg, int codeSizeInPixels = 250)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Renderer = new ZXing.Rendering.BitmapRenderer { Background = Color.Red, Foreground = Color.Blue };
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");//编码问题
            writer.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);

            writer.Options.Height = writer.Options.Width = codeSizeInPixels;
            writer.Options.Margin = 0;//设置边框1            ZXing.Common.BitMatrix bm = writer.Encode(msg);
            Bitmap img = writer.Write(msg);
            return img;
        }

        #endregion
    }
}
