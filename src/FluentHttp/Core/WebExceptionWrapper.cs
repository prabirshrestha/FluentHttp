namespace FluentHttp
{
    using System;
    using System.Net;

#if (!SILVERLIGHT)
    [Serializable]
#endif
#if FLUENTHTTP_CORE_INTERNAL
    internal
#else
    public
#endif
 class WebExceptionWrapper : Exception
    {
        private readonly WebException _webException;

        public WebExceptionWrapper(WebException webException)
            : base(webException == null ? null : webException.Message, webException == null ? null : webException.InnerException)
        {
            _webException = webException;
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Initializes a new instance of the <see cref="WebExceptionWrapper"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected WebExceptionWrapper(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif

        public virtual IHttpWebResponse GetResponse()
        {
            return _webException.Response == null
                       ? null
                       : new HttpWebResponseWrapper((HttpWebResponse)_webException.Response);
        }

        public WebException ActualWebException
        {
            get { return _webException; }
        }
    }
}