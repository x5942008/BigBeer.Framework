using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBeer.Framework.QR_Code
{
    /// <summary>
    /// 保存图片格式
    /// </summary>
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

    /// <summary>
    /// 获取后缀
    /// </summary>
    public class ImgSuffix {
        public static string Getsuffix(ImgType type)
        {
            var suffix = ".png";
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
            return suffix;
        }
    }
}
