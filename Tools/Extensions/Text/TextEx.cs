namespace Tools.Extensions.Text
{
    public static class TextEx
    {
        public static string Tab(this string text) => text.Tab(1);

        public static string Tab(this string text, int count)
        {
            if(text == null || count <= 0)
                return text;

            string tabs = new string('\t', count);

            return text + tabs;
        }
    }
}