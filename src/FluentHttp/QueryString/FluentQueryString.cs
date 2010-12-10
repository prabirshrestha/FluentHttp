namespace FluentHttp
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Fluent QueryString
    /// </summary>
    public class FluentQueryString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentQueryString"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the querystring.
        /// </param>
        /// <param name="value">
        /// The value of the querystring.
        /// </param>
        [ContractVerification(true)]
        public FluentQueryString(string name, string value)
            : this(name, value, true)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            Contract.Ensures(!string.IsNullOrEmpty(this.Name));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentQueryString"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the querystring.
        /// </param>
        /// <param name="value">
        /// The value of the querystring.
        /// </param>
        /// <param name="encode">
        /// Indicates whether to encode the querystring name and value.
        /// </param>
        [ContractVerification(true)]
        public FluentQueryString(string name, string value, bool encode)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            Contract.Ensures(!string.IsNullOrEmpty(this.Name));

            if (encode)
            {
                this.Name = Utils.UrlEncode(name);
                Contract.Assert(!string.IsNullOrEmpty(Name));

                this.Value = Utils.UrlEncode(value);
            }
            else
            {
                this.Name = name;
                Contract.Assert(!string.IsNullOrEmpty(Name));

                this.Value = value;
            }
        }

        /// <summary>
        /// Gets the name of the http header.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value of the http header.
        /// </summary>
        public string Value { get; private set; }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Name));
        }
    }
}