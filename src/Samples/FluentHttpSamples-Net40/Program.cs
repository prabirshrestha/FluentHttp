using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using FluentHttp;

namespace FluentHttpSamples
{
    internal class Program
    {
        private const string AccessToken = "";

        private static void Main(string[] args)
        {
            GetAsync();

#if TPL
            GetAsyncWithTask();
#endif

            Get();

            //var postId = Post("message from fluent http");

            //Console.WriteLine("Check if message was posted in fb.com");
            //Console.ReadKey();

            //Delete(postId);
            //Console.WriteLine("Check if message was deleted in fb.com");

            //UploadPhoto(@"C:\Users\Public\Pictures\Sample Pictures\Koala.jpg", "koala.jpg", "image/jpeg",
            //            "Uploaded using FluentHttp");

            Console.ReadKey();
        }

#if TPL
        private static void GetAsyncWithTask()
        {
            // Stream to save the response to
            var responseSaveStream = new MemoryStream();

            // Prepare the request.
            var request = new FluentHttpRequest()
                .BaseUrl("https://graph.facebook.com")
                .ResourcePath("/4")
                .Method("GET")
                .Headers(h => h.Add("User-Agent", "FluentHttp"))
                .QueryStrings(q => q
                                       .Add("fields", "name,first_name,last_name")
                                       .Add("format", "json"))
                .Proxy(WebRequest.DefaultWebProxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream));

            var task = request.ExecuteTaskAsync();

            task.ContinueWith(
                t =>
                {
                    var ar = t.Result;

                    // seek the save stream to beginning.
                    ar.Response.SaveStream.Seek(0, SeekOrigin.Begin);

                    // Print the response
                    Console.WriteLine("GetAsyncWithTask: ");
                    Console.WriteLine(FluentHttpRequest.ToString(ar.Response.SaveStream));
                });
        }
#endif

        private static void GetAsync()
        {
            // Stream to save the response to
            var responseSaveStream = new MemoryStream();

            // Prepare the request.
            var request = new FluentHttpRequest()
                .BaseUrl("https://graph.facebook.com")
                .ResourcePath("/4")
                .Method("GET")
                .Headers(h => h.Add("User-Agent", "FluentHttp"))
                .QueryStrings(q => q
                                       .Add("fields", "name,first_name,last_name")
                                       .Add("format", "json"))
                .Proxy(WebRequest.DefaultWebProxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream));

            request.ExecuteAsync(ar =>
                                     {
                                         var response = ar.Response;

                                         // seek the save stream to beginning.
                                         response.SaveStream.Seek(0, SeekOrigin.Begin);

                                         // Print the response
                                         Console.WriteLine("GetAsync: ");
                                         Console.WriteLine(FluentHttpRequest.ToString(response.SaveStream));
                                     }, null);
        }

        private static void Get()
        {
            //Stream to save the response to
            var responseSaveStream = new MemoryStream();

            //Prepare the request.
            var request = new FluentHttpRequest()
                .BaseUrl("https://graph.facebook.com")
                .ResourcePath("/4")
                .Method("GET")
                .Headers(h => h.Add("User-Agent", "FluentHttp"))
                .QueryStrings(q => q
                                       .Add("fields", "name,first_name,last_name")
                                       .Add("format", "json"))
                .Proxy(WebRequest.DefaultWebProxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream));

            var asyncResult = request.Execute();

            //seek the save stream to beginning.
            asyncResult.Response.SaveStream.Seek(0, SeekOrigin.Begin);

            //Print the response
            Console.WriteLine("Get: ");
            Console.WriteLine(FluentHttpRequest.ToString(asyncResult.Response.SaveStream));
        }

        private static string Post(string message)
        {
            // Stream to save the response to
            var responseSaveStream = new MemoryStream();

            // Prepare the request.
            var request = new FluentHttpRequest()
                .BaseUrl("https://graph.facebook.com")
                .ResourcePath("/me/feed")
                .Method("POST")
                .Headers(h => h.Add("User-Agent", "FluentHttp"))
                .QueryStrings(q => q
                                       .Add("oauth_token", AccessToken)
                                       .Add("format", "json"))
                .Proxy(WebRequest.DefaultWebProxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream))
                .Body(body =>
                      body.Append(String.Format("{0}={1}", FluentHttpRequest.UrlEncode("message"), message)));

            // Execute the request. Call EndRequest immediately so it behaves synchronously.
            var ar = request.Execute();

            // seek the save stream to beginning.
            responseSaveStream.Seek(0, SeekOrigin.Begin);
            var responseResult = FluentHttpRequest.ToString(responseSaveStream);

            // Convert to json
            var json = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(responseResult);

            // Print the response
            Console.WriteLine("Post: ");
            Console.WriteLine(responseResult);

            return (string)json["id"];
        }

        private static void Delete(string postId)
        {
            // Stream to save the response to
            var responseSaveStream = new MemoryStream();

            // Prepare the request.
            var request = new FluentHttpRequest()
                .BaseUrl("https://graph.facebook.com")
                .ResourcePath(postId)
                .Method("DELETE")
                .Headers(h => h.Add("User-Agent", "FluentHttp"))
                .QueryStrings(q => q
                                       .Add("oauth_token", AccessToken))
                .Proxy(WebRequest.DefaultWebProxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream));

            // Execute the request. Call EndRequest immediately so it behaves synchronously.
            var ar = request.Execute();

            // seek the save stream to beginning.
            responseSaveStream.Seek(0, SeekOrigin.Begin);

            // Print the response
            Console.WriteLine("Delete: ");
            Console.WriteLine(FluentHttpRequest.ToString(responseSaveStream));
        }

        public static string UploadPhoto(string path, string filename, string contentType, string message)
        {
            var parameters = new Dictionary<string, object>();
            parameters["message"] = message;
            parameters["file1"] = new MediaObject { ContentType = contentType, FileName = Path.GetFileName(filename) }
                .SetValue(File.ReadAllBytes(path));

            // Stream to save the response to
            var responseSaveStream = new MemoryStream();

            // Prepare the request.
            var request = new FluentHttpRequest()
                .BaseUrl("https://graph.facebook.com")
                .ResourcePath("/me/photos")
                .Method("POST")
                .Headers(h => h.Add("User-Agent", "FluentHttp"))
                .QueryStrings(q => q.Add("oauth_token", AccessToken))
                .Proxy(WebRequest.DefaultWebProxy)
                .OnResponseHeadersReceived((o, e) => e.SaveResponseIn(responseSaveStream))
                .Body(body => AttachRequestBodyAndUpdateHeader(body.Request, parameters, null));

            // Execute the request. Call EndRequest immediately so it behaves synchronously.
            var ar = request.Execute();

            // seek the save stream to beginning.
            responseSaveStream.Seek(0, SeekOrigin.Begin);
            var responseResult = FluentHttpRequest.ToString(responseSaveStream);

            // Convert to json
            var json = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(responseResult);

            if (ar.Exception != null)
            {
                throw ar.Exception;
            }

            // Print the response
            Console.WriteLine("Upload photo: ");
            Console.WriteLine(responseResult);

            if (ar.InnerException != null)
            {
                throw ar.InnerException;
            }

            return (string)json["id"];
        }

        #region Parameter Helpers

        /// <summary>
        /// The multi-part form prefix characters.
        /// </summary>
        internal const string MultiPartFormPrefix = "--";

        /// <summary>
        /// The multi-part form new line characters.
        /// </summary>
        internal const string MultiPartNewLine = "\r\n";

        internal static IDictionary<string, MediaObject> ExtractMediaObjects(IDictionary<string, object> parameters)
        {
            var mediaObjects = new Dictionary<string, MediaObject>();

            if (parameters == null)
                return mediaObjects;

            foreach (var parameter in parameters)
            {
                if (parameter.Value is MediaObject)
                    mediaObjects.Add(parameter.Key, (MediaObject)parameter.Value);
            }

            foreach (var mediaObject in mediaObjects)
            {
                parameters.Remove(mediaObject.Key);
            }

            return mediaObjects;
        }

        internal static void AttachRequestBodyAndUpdateHeader(FluentHttpRequest request, IDictionary<string, object> parameters, string boundary)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (string.IsNullOrEmpty(boundary))
                boundary = DateTime.Now.Ticks.ToString("x", CultureInfo.InvariantCulture);

            var mediaObjects = ExtractMediaObjects(parameters);

            if (mediaObjects.Count == 0)
            {
                request.Headers(h => h.Add("Content-Type", "application/x-www-form-urlencoded"));

                throw new NotImplementedException();
            }
            else
            {
                request.Headers(h => h.Add("Content-Type", string.Concat("multipart/form-data; boundary=", boundary)));

                // Build up the post message header
                var sb = new StringBuilder();
                foreach (var kvp in parameters)
                {
                    sb.AppendFormat("{0}{1}{2}", MultiPartFormPrefix, boundary, MultiPartNewLine);
                    sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", kvp.Key);
                    sb.AppendFormat("{0}{0}{1}{0}", MultiPartNewLine, kvp.Value);
                }

                request.Body(b => b.Append(sb.ToString()));

                var newLine = Encoding.UTF8.GetBytes(MultiPartNewLine);
                foreach (var kvp in mediaObjects)
                {
                    var sbMediaObject = new StringBuilder();
                    var mediaObject = kvp.Value;

                    if (mediaObject.ContentType == null || mediaObject.GetValue() == null || string.IsNullOrEmpty(mediaObject.FileName))
                        throw new InvalidOperationException("The media object must have a content type, file name, and value set.");

                    sbMediaObject.AppendFormat("{0}{1}{2}", MultiPartFormPrefix, boundary, MultiPartNewLine);
                    sbMediaObject.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", kvp.Key, mediaObject.FileName, MultiPartNewLine);
                    sbMediaObject.AppendFormat("Content-Type: {0}{1}{1}", mediaObject.ContentType, MultiPartNewLine);

                    byte[] fileData = mediaObject.GetValue();
                    Debug.Assert(fileData != null, "The value of MediaObject is null.");

                    request.Body(b => b
                                          .Append(sbMediaObject.ToString())
                                          .Append(fileData)
                                          .Append(newLine));
                }

                request.Body(body => body.Append(Encoding.UTF8.GetBytes(string.Concat(MultiPartNewLine, MultiPartFormPrefix, boundary, MultiPartFormPrefix, MultiPartNewLine))));
            }
        }

        #endregion
    }
}
