using System;
using System.Collections.Generic;
using System.IO;
using FluentHttp;
using System.Net;

namespace FluentHttpSamples
{
    class Program
    {
        const string AccessToken = "";

        static void Main(string[] args)
        {
            Get();

            var postId = Post("message from fluent http");

            Console.WriteLine("Check if message was posted in fb.com");
            Console.ReadKey();

            Delete(postId);
            Console.WriteLine("Check if message was deleted in fb.com");

            Console.ReadKey();
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
            var ar = request.BeginExecute(null, "a");
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
                          {
                              body.Append(string.Format("{0}={1}", "message", FluentHttpRequest.UrlEncode(message)));
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
    }
}
