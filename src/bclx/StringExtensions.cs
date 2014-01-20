namespace bclx
{
    public static class StringExtensions
    {
        public static string ChopFirst(this string input, string head)
        {
            new { input, head }.NullCheck();
            
            if (input.StartsWith(head))
                return input.Substring(head.Length);
            else
                return input;
        }
        
        public static string ChopLast(this string input, string tail)
        {
            new { input, head }.NullCheck();
            if (input.EndsWith(head))
                return input.Substring(0, input.Length - head.Length);
            else
                return input;
        }
    }
}