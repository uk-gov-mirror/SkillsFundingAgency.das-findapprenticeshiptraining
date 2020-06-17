namespace SFA.DAS.FAT.Web.Extensions
{
    public static class LongExtensions
    {
        public static string ToGdsCostFormat(this long value)
        {
            return $"£{value:n0}";
        }
    }
}