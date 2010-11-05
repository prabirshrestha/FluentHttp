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
        private readonly List<FluentHttpHeader> _headers;

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
            _headers = headers ?? new List<FluentHttpHeader>();
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
            _headers.Add(httpHeader);
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
            for (int i = 0; i < _headers.Count; i++)
            {
                var header = _headers[i];
                if (header.Name == name)
                {
                    // adds to the first header found.
                    _headers[i] = new FluentHttpHeader(name, header.Value + value);
                    return this;
                }
            }

            // if header didn't exisit, add it as new one.
            _headers.Add(new FluentHttpHeader(name, value));
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