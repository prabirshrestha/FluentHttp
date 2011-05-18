
namespace FluentHttp
{
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// Represents a Fluent Http Response.
    /// </summary>
    public class FluentHttpResponse
    {
        /// <summary>
        /// The fluent http request.
        /// </summary>
        private readonly FluentHttpRequest _request;

        /// <summary>
        /// The http web response.
        /// </summary>
        private readonly IHttpWebResponse _httpWebResponse;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpResponse"/> class.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentHttpResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpResponse"/> class.
        /// </summary>
        /// <param name="request">
        /// The fluent http web request.
        /// </param>
        /// <param name="httpWebResponse">
        /// The http web response.
        /// </param>
        public FluentHttpResponse(FluentHttpRequest request, IHttpWebResponse httpWebResponse)
            : this()
        {
            _request = request;
            _httpWebResponse = httpWebResponse;
        }

        /// <summary>
        /// Gets the fluent http request.
        /// </summary>
        public virtual FluentHttpRequest Request
        {
            get { return _request; }
        }

        /// <summary>
        /// Gets the http web response..
        /// </summary>
        public virtual IHttpWebResponse HttpWebResponse
        {
            get { return _httpWebResponse; }
        }

        /// <summary>
        /// Gets the reponse save stream.
        /// </summary>
        public virtual Stream SaveStream { get; internal set; }
    }
}