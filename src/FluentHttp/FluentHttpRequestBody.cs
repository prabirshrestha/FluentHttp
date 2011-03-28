namespace FluentHttp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Represnets the fluent http request body
    /// </summary>
    public class FluentHttpRequestBody
    {
        /// <summary>
        /// Internal collection of streams.
        /// </summary>
        private readonly List<Stream> _streams;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpRequestBody"/> class.
        /// </summary>
        public FluentHttpRequestBody()
        {
            _streams = new List<Stream>();
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        public Stream Stream
        {
            get
            {
                return _streams.Count == 0 ? null : new CombinationStream(_streams);
            }
        }

        /// <summary>
        /// Appends a stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The fluent http request body
        /// </returns>
        public FluentHttpRequestBody Append(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            _streams.Add(stream);
            return this;
        }

        /// <summary>
        /// Appends a stream.
        /// </summary>
        /// <param name="data">
        /// The stream.
        /// </param>
        /// <returns>
        /// The fluent http request body.
        /// </returns>
        public FluentHttpRequestBody Append(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return Append(new MemoryStream(data));
        }

        /// <summary>
        /// Appends the string content.
        /// </summary>
        /// <param name="contents">
        /// The contents.
        /// </param>
        /// <param name="encoding">
        /// The encoding.
        /// </param>
        /// <returns>
        /// The fluent http request body.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// if contents is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// if encoding is null.
        /// </exception>
        public FluentHttpRequestBody Append(string contents, Encoding encoding)
        {
            if (string.IsNullOrEmpty(contents))
            {
                throw new ArgumentNullException("contents");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return Append(encoding.GetBytes(contents));
        }

        /// <summary>
        /// Appends string contents.
        /// </summary>
        /// <param name="contents">
        /// The contents.
        /// </param>
        /// <returns>
        /// The fluent http request body.
        /// </returns>
        public FluentHttpRequestBody Append(string contents)
        {
            return Append(contents, Encoding.UTF8);
        }

        /// <summary>
        /// Appends the form parameters.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The fluent http request body.
        /// </returns>
        public FluentHttpRequestBody Append(IDictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return this;
            }

            var sb = new StringBuilder();

            foreach (var parameter in parameters)
            {
                sb.AppendFormat("{0}={1}&", FluentHttpRequest.UrlEncode(parameter.Key), FluentHttpRequest.UrlEncode(parameter.Value.ToString()));
            }

            // remove the last &
            --sb.Length;

            return Append(sb.ToString());
        }
    }
}