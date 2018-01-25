namespace BigBeer.AI.Voice
{
    public enum VoiceType
    {
        /// <summary>
        /// 此类型暂时无法被语音识别
        /// </summary>
        mp3,
        pcm,
        wav,
        amr
    }
    public class VoiceSuffix
    {
        public static string Getsuffix(VoiceType type)
        {
            var suffix = ".mp3";
            switch (type)
            {
                case VoiceType.mp3:
                    suffix = ".mp3";
                    break;
                case VoiceType.pcm:
                    suffix = ".pcm";
                    break;
                case VoiceType.wav:
                    suffix = ".wav";
                    break;
                case VoiceType.amr:
                    suffix = ".wav";
                    break;
                default:
                    suffix = ".mp3";
                    break;
            }
            return suffix;
        }
    }
}