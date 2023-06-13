namespace IPD.Application.Utilities
{
    public class StringUtilities
    {
        /// <summary>
        /// Makes Url from given string
        /// </summary>
        /// <param name="text">input text</param>
        /// <returns>URL</returns>
        public static string MakeLink(string text)
        {
            if (text.ToLower().IndexOf("http://") < 0 &&
                    text.ToLower().IndexOf("https://") < 0)
            {
                text = string.Format("{0}{1}", "http://", text);
            }
            return text;
        }

        /// <summary>
        /// Makes string Capitialize.
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>Capitialized string</returns>
        public static string CapitializeString(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                char[] a = s.ToCharArray();
                a[0] = char.ToUpper(a[0]);
                return new string(a);
            }
            else
            { return s; }
        }
    }
}
