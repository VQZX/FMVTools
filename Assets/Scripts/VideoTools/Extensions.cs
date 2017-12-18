namespace VideoTools
{
    public static class Extensions
    {
        public static string ToTime(this float timeInSeconds)
        {
            int minutes = (int) (timeInSeconds / 60f);
            int seconds = (int) (timeInSeconds % 60);
            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}