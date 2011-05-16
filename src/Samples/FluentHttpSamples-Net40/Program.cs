using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentHttp;
using System.Net;

namespace FluentHttpSamples
{
    class Program
    {
        const string AccessToken = "";

        static void Main(string[] args)
        {
            GetAsync();

#if TPL
            GetAsyncWithTask();
#endif

            Get();

            var postId = Post("message from fluent http");

            Console.WriteLine("Check if message was posted in fb.com");
            Console.ReadKey();

            Delete(postId);
            Console.WriteLine("Check if message was deleted in fb.com");

            //UploadPhoto(@"C:\Users\Public\Pictures\Sample Pictures\Koala.jpg", "koala.jpg", "image/jpeg", "Uploaded using FluentHttp");

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
                .OnResponseHeadersReceived((o, e) => e.ResponseSaveStream = responseSaveStream);

            var task = request.ToTask();

            task.ContinueWith(
                t =>
                {
                    var response = t.Result;

                    // seek the save stream to beginning.
                    response.SaveStream.Seek(0, SeekOrigin.Begin);

                    // Print the response
                    Console.WriteLine("GetAsyncWithTask: ");
                    Console.WriteLine(FluentHttpRequest.ToString(response.SaveStream));
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
                .OnResponseHeadersReceived((o, e) => e.ResponseSaveStream = responseSaveStream);

            request.BeginExecute(ar =>
                                     {
                                         var response = request.EndExecute(ar);

                                         // seek the save stream to beginning.
                                         response.SaveStream.Seek(0, SeekOrigin.Begin);

                                         // Print the response
                                         Console.WriteLine("GetAsync: ");
                                         Console.WriteLine(FluentHttpRequest.ToString(response.SaveStream));
                                     }, null);
        }

        private static void Get()
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
                .OnResponseHeadersReceived((o, e) => e.ResponseSaveStream = responseSaveStream);

            // Execute the request. Call EndRequest immediately so it behaves synchronously.
            var ar = request.BeginExecute(null, null);
            var response = request.EndExecute(ar);

            // seek the save stream to beginning.
            responseSaveStream.Seek(0, SeekOrigin.Begin);

            // Print the response
            Console.WriteLine("Get: ");
            Console.WriteLine(FluentHttpRequest.ToString(responseSaveStream));
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
                .OnResponseHeadersReceived((o, e) => e.ResponseSaveStream = responseSaveStream)
                .Body(body =>
                      body.Append(string.Format("{0}={1}", FluentHttpRequest.UrlEncode("message"), message)));

            // Execute the request. Call EndRequest immediately so it behaves synchronously.
            var ar = request.BeginExecute(null, null);
            var response = request.EndExecute(ar);

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
                .OnResponseHeadersReceived((o, e) => e.ResponseSaveStream = responseSaveStream);

            // Execute the request. Call EndRequest immediately so it behaves synchronously.
            var ar = request.BeginExecute(null, null);
            var response = request.EndExecute(ar);

            // seek the save stream to beginning.
            responseSaveStream.Seek(0, SeekOrigin.Begin);

            // Print the response
            Console.WriteLine("Delete: ");
            Console.WriteLine(FluentHttpRequest.ToString(responseSaveStream));
        }

        public static string UploadPhoto(string path, string filename, string contentType, string message)
        {
            // Stream to save the response to
            var responseSaveStream = new MemoryStream();

            string multipartBoundary = DateTime.Now.Ticks.ToString("x", System.Globalization.CultureInfo.InvariantCulture);

            // Prepare the request.
            var request = new FluentHttpRequest()
                .BaseUrl("https://graph.facebook.com")
                .ResourcePath("/me/photos")
                .Method("POST")
                .Headers(h => h
                    .Add("User-Agent", "FluentHttp")
                    .Add("Content-Type", string.Concat("multipart/form-data; boundary=", multipartBoundary)))
                .QueryStrings(q => q
                                       .Add("oauth_token", AccessToken))
                .Proxy(WebRequest.DefaultWebProxy)
                .OnResponseHeadersReceived((o, e) => e.ResponseSaveStream = responseSaveStream)
                .Body(body =>
                          {
                              // Build up the post message header
                              var sb = new StringBuilder();
                              const string multipartFormPrefix = "--";
                              const string multipartNewline = "\r\n";

                              Action<StringBuilder, string, string> formData =
                                  (fd, key, value) =>
                                  {
                                      fd.AppendFormat("{0}{1}{2}", multipartFormPrefix, multipartBoundary, multipartNewline);
                                      fd.AppendFormat("Content-Disposition: form-data; name=\"{0}\"", key);
                                      fd.AppendFormat("{0}{1}", multipartNewline, multipartNewline);
                                      fd.Append(value);
                                      fd.Append(multipartNewline);
                                  };

                              formData(sb, "message", message);

                              sb.AppendFormat("{0}{1}{2}", multipartFormPrefix, multipartBoundary, multipartNewline);
                              sb.AppendFormat("Content-Disposition: form-data; filename=\"{0}\"{1}", filename, multipartNewline);
                              sb.AppendFormat("Content-Type: {0}{1}{2}", contentType, multipartNewline, multipartNewline);

                              byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sb.ToString());
                              byte[] fileData = File.ReadAllBytes(path);
                              byte[] boundaryBytes = Encoding.UTF8.GetBytes(string.Concat(multipartNewline, multipartFormPrefix, multipartBoundary, multipartFormPrefix, multipartNewline));

                              // Combine all bytes to post
                              var postData = new byte[postHeaderBytes.Length + fileData.Length + boundaryBytes.Length];
                              Buffer.BlockCopy(postHeaderBytes, 0, postData, 0, postHeaderBytes.Length);
                              Buffer.BlockCopy(fileData, 0, postData, postHeaderBytes.Length, fileData.Length);
                              Buffer.BlockCopy(boundaryBytes, 0, postData, postHeaderBytes.Length + fileData.Length, boundaryBytes.Length);

                              body.Append(postData);
                          });

            // Execute the request. Call EndRequest immediately so it behaves synchronously.
            var ar = request.BeginExecute(null, null);
            var response = request.EndExecute(ar);

            // seek the save stream to beginning.
            responseSaveStream.Seek(0, SeekOrigin.Begin);
            var responseResult = FluentHttpRequest.ToString(responseSaveStream);

            // Convert to json
            var json = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(responseResult);

            // Print the response
            Console.WriteLine("Upload photo: ");
            Console.WriteLine(responseResult);

            return (string)json["id"];
        }
    }
}
