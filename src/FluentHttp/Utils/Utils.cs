namespace FluentHttp
{
    using FluentHttp.External;

    public class Utils
    {
        public static string UrlDecode(string input)
        {
            return HttpUtility.UrlDecode(input);
        }

        public static string UrlEncode(string input)
        {
            return HttpUtility.UrlEncode(input);
        }

        public static string HtmlDecode(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }

        public static string HtmlEncode(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }
    }
}