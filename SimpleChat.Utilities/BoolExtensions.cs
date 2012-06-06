namespace SimpleChat.Utilities
{
    public static class BoolExtensions
    {
        public static string ToYesNoString(this bool b)
        {
            return b.ToStringWithOptions("Yes", "No");
        }

        public static string ToStringWithOptions(this bool b, string trueString, string falseString)
        {
            if (b)
                return trueString;
            return falseString;
        }
    }
}
