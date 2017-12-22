namespace VideoTools
{
    public static class NumberExtensions
    {
        public static string ToTime(this float timeInSeconds)
        {
            int minutes = (int) (timeInSeconds / 60f);
            int seconds = (int) (timeInSeconds % 60);
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

        public static string ToTime(this double timeInSeconds)
        {
            return ((float) timeInSeconds).ToTime();
        }

        public static float TimeToFloat(this string stringifiedTime)
        {
            string[] split = stringifiedTime.Split(':');
            // we currently only do it for minutes and seconds anyway
            if (split.Length != 2)
            {
                return -1f;
            }
            float result = 0;
            float parsed = 0;
            if (!float.TryParse(split[0], out parsed))
            {
                return -1f;
            }
            result += parsed * 60;
            if (!float.TryParse(split[1], out parsed))
            {
                return -1f;
            }
            result += parsed;
            return result;
        }
    }
}