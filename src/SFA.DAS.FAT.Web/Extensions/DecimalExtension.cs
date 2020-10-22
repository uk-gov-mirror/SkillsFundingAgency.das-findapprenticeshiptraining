namespace SFA.DAS.FAT.Web.Extensions
{
    public static class DecimalExtension
    {
        public static string FormatDistance(this decimal value)
        {
            var returnValue = value.ToString("F1");

            returnValue = returnValue.TrimEnd('0');
            returnValue = returnValue.TrimEnd('.');
            return returnValue;
        }
    }
}