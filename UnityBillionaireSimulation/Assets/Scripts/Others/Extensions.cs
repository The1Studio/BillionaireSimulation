namespace TheOneStudio.HyperCasual.Others
{
    using System.Globalization;

    public static class Extensions
    {
        public static int ToMilliseconds(this float second) { return (int)(second * 1000); }
        public static string ToFormattedRewardCash(this long cash)
        {
            var result = cash.ToString("N0", new NumberFormatInfo { NumberGroupSeparator = "." });
            return result;
        }
    }
}