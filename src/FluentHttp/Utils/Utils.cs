namespace FluentHttp
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;

    /// <summary>
    /// Fluent Http Utilities
    /// </summary>
    public class Utils
    {
        public static string UrlDecode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlDecode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlDecode(input);
#else
            return HttpUtility.UrlDecode(input);
#endif
        }

        public static string UrlEncode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.UrlEncode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlEncode(input);
#else
            return HttpUtility.UrlEncode(input);
#endif
        }

        public static string HtmlDecode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.HtmlDecode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.HtmlDecode(input);
#else
            return HttpUtility.HtmlDecode(input);
#endif
        }

        public static string HtmlEncode(string input)
        {
#if WINDOWS_PHONE
            return System.Net.HttpUtility.HtmlEncode(input);
#elif SILVERLIGHT
            return System.Windows.Browser.HttpUtility.HtmlEncode(input);
#else
            return HttpUtility.HtmlEncode(input);
#endif
        }

        /// <summary>
        /// Merges two dictionaries.
        /// </summary>
        /// <param name="first">Default values, only used if second does not contain a value.</param>
        /// <param name="second">Every value of the merged object is used.</param>
        /// <returns>The merged dictionary</returns>
        public static IDictionary<string, object> Merge(IDictionary<string, object> first, IDictionary<string, object> second)
        {
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            first = first ?? new Dictionary<string, object>();
            second = second ?? new Dictionary<string, object>();
            var merged = new Dictionary<string, object>();

            foreach (var kvp in second)
            {
                merged.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in first)
            {
                if (!merged.ContainsKey(kvp.Key))
                {
                    merged.Add(kvp.Key, kvp.Value);
                }
            }

            return merged;
        }

        /// <summary>
        /// Converts stream to string.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// </returns>
        public static string ToString(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Adds a forward slash if not present.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// Returns a string starting with /.
        /// </returns>
        public static string AddStartingSlashInNotPresent(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "/";
            }

            // if not null or empty
            if (input[0] != '/')
            {
                // if doesn't start with / then add /
                return "/" + input;
            }
            else
            {
                // else return the original input.
                return input;
            }
        }
    }
}