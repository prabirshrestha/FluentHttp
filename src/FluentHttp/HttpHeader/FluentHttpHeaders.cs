namespace FluentHttp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

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
        private readonly List<FluentHttpHeader> headers = new List<FluentHttpHeader>();

        /// <summary>
        /// List of special http headers.
        /// </summary>
        private static readonly List<string> SpecialHeaders = new List<string>
                                                                  {
                                                                      "accept",             // Set by the Accept property.
                                                                      "connection",         // Set by the Connection property and KeepAlive property.
                                                                      "content-length",     // Set by the ContentLength property.
                                                                      "content-type",       // Set by the ContentType property
                                                                      "expect",             // Set by the Expect property.
                                                                      "date",               // Set by the Date property.
                                                                      "host",               // Set by the Host property.
                                                                      "if-modified-since",  // Set by the IfModifiedSince property.
                                                                      "range",              // Set by the AddRange method.
                                                                      "referer",            // Set by the Referer property.
                                                                      "transfer-encoding",  // Set by the TransferEncoding property (the SendChunked property must be true).
                                                                      "user-agent",         // Set by the UserAgent property.
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
        [ContractVerification(true)]
        [Pure]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static int IsSpecialHeader(string headerName)
        {
            Contract.Requires(!string.IsNullOrEmpty(headerName));
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < SpecialHeaders.Count);

            return SpecialHeaders.FindIndex(h => h.Equals(headerName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds the specified <see cref="FluentHttpHeader"/>.
        /// </summary>
        /// <param name="httpHeader">
        /// The http header.
        /// </param>
        /// <returns>
        /// Returns of <see cref="FluentHttpHeaders"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpHeaders Add(FluentHttpHeader httpHeader)
        {
            Contract.Requires(httpHeader != null);
            Contract.Requires(!string.IsNullOrEmpty(httpHeader.Name));
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

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
        [ContractVerification(true)]
        public FluentHttpHeaders Add(string name, string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            return Add(new FluentHttpHeader(name, value));
        }

        /// <summary>
        /// Appends the value of the http header if exists otherwise create a new one.
        /// </summary>
        /// <param name="name">
        /// The http header name.
        /// </param>
        /// <param name="value">
        /// The http header value.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpHeaders"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpHeaders Append(string name, string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            var index = FindHeader(name);

            if (index >= 0)
            {
                // if header exists append value.
                var header = this.headers[index];
                Contract.Assume(header != null);
                this.headers[index] = new FluentHttpHeader(name, header.Value + value);
                return this;
            }

            // if header doesn't exist do normal add.
            return Add(name, value);
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerator<FluentHttpHeader> GetEnumerator()
        {
            return this.headers.GetEnumerator();
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

        #region Hide defualt object methods

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>. 
        ///                 </param><exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.
        /// </exception><filterpriority>2</filterpriority>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

#pragma warning disable 0108
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), EditorBrowsable(EditorBrowsableState.Never)]
        public Type GetType()
        {
            return base.GetType();
        }
#pragma warning restore 0108

        #endregion

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvariantObject()
        {
            Contract.Invariant(this.headers != null);
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
        [ContractVerification(true)]
        private int FindHeader(string headerName)
        {
            Contract.Requires(!string.IsNullOrEmpty(headerName));
            Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < this.headers.Count);

            return this.headers.FindIndex(h => h.Name.Equals(headerName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Adds the custom http header which doesn't belong to the specific headers list.
        /// </summary>
        /// <param name="httpHeader">
        /// The http header.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpHeaders"/>.
        /// </returns>
        [ContractVerification(true)]
        private FluentHttpHeaders AddCustomHeader(FluentHttpHeader httpHeader)
        {
            Contract.Requires(httpHeader != null);
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            this.headers.Add(httpHeader);

            return this;
        }

        /// <summary>
        /// Adds special http header.
        /// </summary>
        /// <param name="httpHeader">
        /// The http header.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpHeaders"/>.
        /// </returns>
        [ContractVerification(true)]
        private FluentHttpHeaders AddSpecialHeader(FluentHttpHeader httpHeader)
        {
            Contract.Requires(httpHeader != null);
            Contract.Requires(!string.IsNullOrEmpty(httpHeader.Name));
            Contract.Ensures(Contract.Result<FluentHttpHeaders>() != null);

            var index = FindHeader(httpHeader.Name);

            if (index >= 0)
            {
                // if found
                this.headers[index] = httpHeader;
                return this;
            }

            // if not found
            return AddCustomHeader(httpHeader);
        }
    }
}