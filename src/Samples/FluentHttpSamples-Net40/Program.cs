using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            GetSample();

            Console.ReadKey();
        }

        private static void GetSample()
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
            request.EndExecute(ar);

            // seek the save stream to beginning.
            responseSaveStream.Seek(0, SeekOrigin.Begin);

            // Print the response
            Console.WriteLine("Get: ");
            Console.WriteLine(FluentHttpRequest.ToString(responseSaveStream));
        }
    }
}
