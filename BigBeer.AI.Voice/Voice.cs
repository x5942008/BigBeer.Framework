using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Baidu.Aip.Speech;
using Microsoft.Extensions.Configuration;

namespace BigBeer.AI.Voice
{
    public class Voices
    {
        private static IConfigurationRoot build => new ConfigurationBuilder()
        .AddJsonFile("appsetting.json", false, true)
        .Build();

        private static string apikey => build["Voice1:API_KEY"];
        private static string secret_key => build["Voice1:SECRET_KEY"];
        private static Asr asr => new Asr(apikey, secret_key);
        private static Tts tts => new Tts(apikey, secret_key);

        /// <summary>
        /// 识别语音
        /// </summary>
        /// <param name="url">文件地址</param>
        /// <param name="format">目前格式仅仅支持pcm，wav或amr，如填写mp3即会有此错误</param>
        /// <returns></returns>
        public static string AsrData(string url, string format)
        {
            var data = File.ReadAllBytes(url);
            var result = asr.Recognize(data, format, 16000);
            return result.ToString();
        }

        /// <summary>
        /// 语音合成
        /// </summary>
        /// <param name="content">合成内容</param>
        /// <param name="path">存储地址</param>
        /// <param name="suffix">目前格式仅仅支持pcm，wav或amr，如填写mp3即会有此错误</param>
        /// <param name="speed">语速</param>
        /// <param name="vol">声量</param>
        /// <param name="per">0为女声，1为男声，3为情感合成男，4为情感女</param>
        public static void Tts(string content, string path, string suffix = "wav", int per = 1, int speed = 5, int vol = 7)
        {
            string sf = "wav";
            int p = 1;
            int s = 5;
            int v = 7;
            if (per != 1) p = per;
            if (speed != 5) s = speed;
            if (per != 7) v = vol;

            // 可选参数
            var option = new Dictionary<string, object>()
    {
        {"spd", s}, // 语速
        {"vol", v}, // 音量
        {"per", p}  // 发音人，4：情感度丫丫童声
    };
            var result = tts.Synthesis(content, option);

            if (!string.IsNullOrEmpty(suffix))
                sf = suffix;
            if (result.ErrorCode == 0)  // 或 result.Success
            {
                File.WriteAllBytes(path + $".{sf}", result.Data);
            }
        }
    }
}
