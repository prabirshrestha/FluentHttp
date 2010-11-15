
namespace FluentHttp
{
    using System;
    using System.IO;
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

        public static void CopyStreamToStream(
            Stream source, Stream destination,
            Action<Stream, Stream, Exception> completed,
            int bufferSize)
        {
            var buffer = new byte[bufferSize];

            Action<Exception> done =
                e =>
                {
                    if (completed != null)
                        completed(source, destination, e);
                };

            AsyncCallback rc = null;
            rc = readResult =>
                     {
                         try
                         {
                             int read = source.EndRead(readResult);
                             if (read > 0)
                             {
                                 destination.BeginWrite(buffer, 0, read,
                                                        writeResult =>
                                                        {
                                                            try
                                                            {
                                                                destination.EndWrite(writeResult);
                                                                source.BeginRead(buffer, 0, buffer.Length, rc, null);
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                done(ex);
                                                            }
                                                        }, null);
                             }
                             else
                             {
                                 done(null);
                             }
                         }
                         catch (Exception ex)
                         {
                             done(ex);
                         }
                     };
            source.BeginRead(buffer, 0, buffer.Length, rc, null);
        }
    }
}