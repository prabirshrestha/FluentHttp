namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here."), ContractClassFor(typeof(IFluentHttpRequest))]
    public class FluentHttpRequestContracts : IFluentHttpRequest
    {
        public string BaseUrl
        {
            get
            {
                Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

                return default(string);
            }
        }

        public IFluentHttpRequest ResourcePath(string resourcePath)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public string GetResourcePath()
        {
            return default(string);
        }

        public IFluentHttpRequest Method(string method)
        {
            Contract.Requires(!string.IsNullOrEmpty(method));

            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public string GetMethod()
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            return default(string);
        }

        public IAsyncResult BeginRequest(AsyncCallback callback, object state)
        {
            Contract.Ensures(Contract.Result<IAsyncResult>() != null);

            return default(IAsyncResult);
        }

        public IFluentHttpResponse EndRequest(IAsyncResult asyncResult)
        {
            Contract.Ensures(asyncResult != null);
            Contract.Ensures(Contract.Result<IFluentHttpResponse>() != null);

            return default(IFluentHttpResponse);
        }

        public IFluentHttpRequest Headers(Action<FluentHttpHeaders> headers)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public FluentHttpHeaders GetHeaders()
        {
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            return default(FluentHttpHeaders);
        }

        public IFluentHttpRequest QueryStrings(Action<FluentQueryStrings> queryStrings)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public FluentQueryStrings GetQueryStrings()
        {
            Contract.Ensures(Contract.Result<FluentQueryStrings>() != null);

            return default(FluentQueryStrings);
        }

        public IFluentHttpRequest Body(Action<FluentHttpRequestBody> body)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public FluentHttpRequestBody GetBody()
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            return default(FluentHttpRequestBody);
        }

        public IFluentHttpRequest Cookies(Action<FluentCookies> cookies)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public FluentCookies GetCookies()
        {
            Contract.Ensures(Contract.Result<FluentCookies>() != null);

            return default(FluentCookies);
        }

        public IFluentHttpRequest Proxy(IWebProxy proxy)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public IWebProxy GetProxy()
        {
            return default(IWebProxy);
        }

        public IFluentHttpRequest Credentials(ICredentials credentials)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public ICredentials GetCredentials()
        {
            return default(ICredentials);
        }

        public IFluentHttpRequest Timeout(int timeout)
        {
            Contract.Requires(timeout >= 0);
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public int GetTimeout()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);

            return default(int);
        }

        public IFluentHttpRequest BufferSize(int bufferSize)
        {
            Contract.Requires(bufferSize >= 1);
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public int GetBufferSize()
        {
            Contract.Ensures(Contract.Result<int>() >= 1);

            return default(int);
        }

        public IFluentHttpRequest AuthenticateUsing(IFluentAuthenticator authenticator)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public IFluentHttpRequest AuthenticateUsing(Func<IFluentAuthenticator> authenticator)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public IFluentAuthenticator GetAuthenticator()
        {
            return default(IFluentAuthenticator);
        }

        public IFluentHttpRequest SaveTo(Stream saveStream, bool seekToBeginingWhenDone)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public IFluentHttpRequest SaveTo(Stream saveStream)
        {
            Contract.Ensures(Contract.Result<IFluentHttpRequest>() != null);

            return default(IFluentHttpRequest);
        }

        public Stream GetSaveStream()
        {
            return default(Stream);
        }

#if !(NET35 || NET20)

        public System.Threading.Tasks.Task<IFluentHttpResponse> ToTask(object state, System.Threading.Tasks.TaskCreationOptions taskCreationOptions)
        {
            Contract.Ensures(Contract.Result<System.Threading.Tasks.Task<IFluentHttpResponse>>() != null);

            return default(System.Threading.Tasks.Task<IFluentHttpResponse>);
        }

        public System.Threading.Tasks.Task<IFluentHttpResponse> ToTask(object state)
        {
            Contract.Ensures(Contract.Result<System.Threading.Tasks.Task<IFluentHttpResponse>>() != null);

            return default(System.Threading.Tasks.Task<IFluentHttpResponse>);
        }

        public System.Threading.Tasks.Task<IFluentHttpResponse> ToTask()
        {
            Contract.Ensures(Contract.Result<System.Threading.Tasks.Task<IFluentHttpResponse>>() != null);

            return default(System.Threading.Tasks.Task<IFluentHttpResponse>);
        }

#endif
    }
}