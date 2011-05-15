# FluentHttp

## Overview
FluentHttp is a light weight library aimed to ease the development of your rest client with
consistent api throughout different frameworks whether you are using .net 4.0 or silverlight or
window phone. (It is not aimed to be used directly by the developers, but rather for creating 
a rest client wrapper, such as for Facebook, Github, Twitter, Google etc.)

## Supported Frameworks

* .NET 4.0 (client profile supported) ~50kb
* .NET 3.5 (client profile supported) ~50kb
* Silverlight 4.0  ~31kb
* Windows Phone 7  ~19kb

## Getting Started
Reference appropriate FluentHttp.dll depending on your framework.

## Samples
Here are some samples to get started with on FluentHttp.

### Using with BeginXXX and EndXXX pattern

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

### Using with Task Parallel Library (TPL)
Note: supported only in .NET 4.0

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

### Making synchronous requests
Unlike most of the rest libraries, FluentHttp only supports async web requests. But you 
can still make it behave like a synchronous request by calling EndExecute right after BeginExecute.

    // Execute the request then call EndRequest immediately so it behaves synchronously.
    var response = request.EndExecute(request.BeginExecute(null, null));

## License
Apache License 2.0
