namespace FluentHttp
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Http Headers for <see cref="FluentHttpRequest"/>
    /// </summary>
    public class FluentHttpHeaders : IEnumerable<FluentHttpHeader>
    {
        /// <summary>
        /// List of http headers.
        /// </summary>
        /// <remarks>
        /// Since headers can be non unique, we need to make it list rather than dictionary.
        /// </remarks>
        private readonly List<FluentHttpHeader> _customHeaders;

        // http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.headers.aspx
        private Dictionary<string, string> SpecialHeaders = new Dictionary<string, string>
                                                       {
                                                           {"accept",            null}, // Set by the Accept property.
                                                           {"connection",        null}, // Set by the Connection property and KeepAlive property.
                                                           {"content-length",    null}, // Set by the ContentLength property.
                                                           {"content-type",      null}, // Set by the ContentType property
                                                           {"expect",            null}, // Set by the Expect property.
                                                           {"date",              null}, // Set by the Date property.
                                                           {"host",              null}, // Set by the Host property.
                                                           {"if-modified-since", null}, // Set by the IfModifiedSince property.
                                                           {"range",             null}, // Set by the AddRange method.
                                                           {"referer",           null}, // Set by the Referer property.
                                                           {"transfer-encoding", null}, // Set by the TransferEncoding property (the SendChunked property must be true).
                                                           {"user-agent",        null}  // Set by the UserAgent property.
                                                       };

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpHeaders"/> class.
        /// </summary>
        public FluentHttpHeaders()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpHeaders"/> class.
        /// </summary>
        /// <param name="headers">
        /// List of http headers.
        /// </param>
        public FluentHttpHeaders(List<FluentHttpHeader> headers)
        {
            // need to do copy ctor
            _customHeaders = headers ?? new List<FluentHttpHeader>();
        }

        /// <summary>
        /// Adds the specified <see cref="FluentHttpHeader"/>.
        /// </summary>
        /// <param name="httpHeader">
        /// The http header.
        /// </param>
        /// <returns>
        /// Current instance of <see cref="FluentHttpHeaders"/>.
        /// </returns>
        public FluentHttpHeaders Add(FluentHttpHeader httpHeader)
        {
            var name = httpHeader.Name.ToLower();

            if (SpecialHeaders.ContainsKey(name))
                SpecialHeaders[name] = httpHeader.Value;
            else
                _customHeaders.Add(httpHeader);

            return this;
        }

        /// <summary>
        /// Adds the http header with the specified name and value.
        /// </summary>
        /// <param name="name">
        /// The name of the http header.
        /// </param>
        /// <param name="value">
        /// The value of the http header.
        /// </param>
        /// <returns>
        /// Current instance of <see cref="FluentHttpHeaders"/>.
        /// </returns>
        public FluentHttpHeaders Add(string name, string value)
        {
            return Add(new FluentHttpHeader(name, value));
        }

        /// <summary>
        /// Appends the value of the http header if exists otherwise create a new one.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// </returns>
        public FluentHttpHeaders Append(string name, string value)
        {
            var lName = name.ToLower();
            if (SpecialHeaders.ContainsKey(lName))
            {
                SpecialHeaders[name] += value;
                return this;
            }

            for (int i = 0; i < _customHeaders.Count; i++)
            {
                var header = _customHeaders[i];
                if (header.Name == name)
                {
                    // adds to the first header found.
                    _customHeaders[i] = new FluentHttpHeader(name, header.Value + value);
                    return this;
                }
            }

            // if header didn't exisit, add it as new one.
            _customHeaders.Add(new FluentHttpHeader(name, value));
            return this;
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<FluentHttpHeader> GetEnumerator()
        {
            return _customHeaders.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}