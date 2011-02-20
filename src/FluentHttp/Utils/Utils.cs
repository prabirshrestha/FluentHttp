namespace FluentHttp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// Fluent Http Utilities
    /// </summary>
    public class Utils
    {
        #region DateTime Utils

        /// <summary>
        /// Gets the epoch time.
        /// </summary>
        /// <value>The epoch time.</value>
        public static DateTime Epoch
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); }
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The unix time.</param>
        /// <returns>The DateTime object.</returns>
        public static DateTime FromUnixTime(double unixTime)
        {
            return Epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Converts a unix time string to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The string representation of the unix time.</param>
        /// <returns>The DateTime object.</returns>
        public static DateTime FromUnixTime(string unixTime)
        {
            double d;
            if (!double.TryParse(unixTime, out d))
            {
                return FromUnixTime(0);
            }

            return FromUnixTime(d);
        }

        /// <summary>
        /// Converts a DateTime object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix date time.</returns>
        public static double ToUnixTime(DateTime dateTime)
        {
            Contract.Requires(dateTime >= Epoch);
            return (double)(dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }

        /// <summary>
        /// Converts a DateTimeOffset object to unix time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The unix date time.</returns>
        public static double ToUnixTime(DateTimeOffset dateTime)
        {
            Contract.Requires(dateTime >= Epoch);
            return (double)(dateTime.ToUniversalTime() - Epoch).TotalSeconds;
        }

        /// <summary>
        /// Converts to specified <see cref="DateTime"/> to ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ).
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// Returns the string representation of date time in ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ).
        /// </returns>
        public static string ToIso8601FormattedDateTime(DateTime dateTime)
        {
            Contract.Requires(dateTime != null);
            return dateTime.ToString("o");
        }

        /// <summary>
        /// Converts ISO-8601 format (yyyy-MM-ddTHH:mm:ssZ) date time to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="iso8601DateTime">
        /// The iso 8601 formatted date time.
        /// </param>
        /// <returns>
        /// Returns the <see cref="DateTime"/> equivalent to the ISO-8601 formatted date time. 
        /// </returns>
        public static DateTime FromIso8601FormattedDateTime(string iso8601DateTime)
        {
            Contract.Requires(!string.IsNullOrEmpty(iso8601DateTime));
            return DateTime.ParseExact(iso8601DateTime, "o", System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion

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
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            first = first ?? new Dictionary<TKey, TValue>();
            second = second ?? new Dictionary<TKey, TValue>();
            var merged = new Dictionary<TKey, TValue>();

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

        #region Encryption Decryption Helper methods

        internal static byte[] ComputeHmacSha1Hash(byte[] data, byte[] key)
        {
            Contract.Requires(data != null);
            Contract.Requires(key != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);

            using (var crypto = new HMACSHA1(key))
            {
                return crypto.ComputeHash(data);
            }
        }

        #endregion

        #region QueryString Utils

        /// <summary>
        /// Parse a URL query and fragment parameters into a key-value bundle.
        /// </summary>
        /// <param name="query">
        /// The URL query to parse.
        /// </param>
        /// <returns>
        /// Returns a dictionary of keys and values for the querystring.
        /// </returns>
        public static IDictionary<string, object> ParseUrlQueryString(string query)
        {
            Contract.Ensures(Contract.Result<IDictionary<string, object>>() != null);

            var result = new Dictionary<string, object>();

            // if string is null, empty or whitespace
            if (string.IsNullOrEmpty(query) || query.Trim().Length == 0)
            {
                return result;
            }

            string decoded = HtmlDecode(query);
            int decodedLength = decoded.Length;
            int namePos = 0;
            bool first = true;

            while (namePos <= decodedLength)
            {
                int valuePos = -1, valueEnd = -1;
                for (int q = namePos; q < decodedLength; q++)
                {
                    if (valuePos == -1 && decoded[q] == '=')
                    {
                        valuePos = q + 1;
                    }
                    else if (decoded[q] == '&')
                    {
                        valueEnd = q;
                        break;
                    }
                }

                if (first)
                {
                    first = false;
                    if (decoded[namePos] == '?')
                    {
                        namePos++;
                    }
                }

                string name, value;
                if (valuePos == -1)
                {
                    name = null;
                    valuePos = namePos;
                }
                else
                {
                    name = UrlDecode(decoded.Substring(namePos, valuePos - namePos - 1));
                }

                if (valueEnd < 0)
                {
                    namePos = -1;
                    valueEnd = decoded.Length;
                }
                else
                {
                    namePos = valueEnd + 1;
                }

                value = UrlDecode(decoded.Substring(valuePos, valueEnd - valuePos));

                if (!string.IsNullOrEmpty(name))
                {
                    result[name] = value;
                }

                if (namePos == -1)
                {
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}