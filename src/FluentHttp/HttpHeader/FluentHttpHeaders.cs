namespace FluentHttp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;

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
        private readonly List<FluentHttpHeader> _headers = new List<FluentHttpHeader>();

        private static readonly List<string> SpecialHeaders = new List<string>
                                                        {
                                                           "accept",            // Set by the Accept property.
                                                           "connection",        // Set by the Connection property and KeepAlive property.
                                                           "content-length",    // Set by the ContentLength property.
                                                           "content-type",      // Set by the ContentType property
                                                           "expect",            // Set by the Expect property.
                                                           "date",              // Set by the Date property.
                                                           "host",              // Set by the Host property.
                                                           "if-modified-since", // Set by the IfModifiedSince property.
                                                           "range",             // Set by the AddRange method.
                                                           "referer",           // Set by the Referer property.
                                                           "transfer-encoding", // Set by the TransferEncoding property (the SendChunked property must be true).
                                                           "user-agent",        // Set by the UserAgent property.
                                                       };

        /// <summary>
        /// Checks if the header is special.
        /// </summary>
        /// <param name="headerName">
        /// The header name.
        /// </param>
        /// <returns>
        /// Returns the zero-based index of the first occurrence within the entire headers.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int IsSpecialHeader(string headerName)
        {
            return SpecialHeaders.FindIndex(h => h.Equals(headerName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Finds the specified header's index.
        /// </summary>
        /// <param name="headerName">
        /// The header name.
        /// </param>
        /// <returns>
        /// Returns the zero-based index of the first occurrence within the entire headers.
        /// </returns>
        private int FindHeader(string headerName)
        {
            return _headers.FindIndex(h => h.Name.Equals(headerName, StringComparison.OrdinalIgnoreCase));
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
            return IsSpecialHeader(httpHeader.Name) >= 0 ? AddSpecialHeader(httpHeader) : AddCustomHeader(httpHeader);
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
            var index = FindHeader(name);

            if (index >= 0)
            {
                // if header exists append value.
                var header = _headers[index];
                _headers[index] = new FluentHttpHeader(name, header.Value + value);
                return this;
            }

            // if header doesn't exist do normal add.
            return Add(name, value);
        }

        private FluentHttpHeaders AddCustomHeader(FluentHttpHeader httpHeader)
        {
            _headers.Add(httpHeader);
            return this;
        }

        private FluentHttpHeaders AddSpecialHeader(FluentHttpHeader httpHeader)
        {
            var index = FindHeader(httpHeader.Name);

            if (index >= 0)
            {
                // if found
                _headers[index] = httpHeader;
                return this;
            }

            // if not found
            return AddCustomHeader(httpHeader);

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
            return _headers.GetEnumerator();
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