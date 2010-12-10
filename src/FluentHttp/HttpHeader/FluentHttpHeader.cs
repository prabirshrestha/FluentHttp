namespace FluentHttp
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Http Header for Fluent Http
    /// </summary>
    public class FluentHttpHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpHeader"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the http header.
        /// </param>
        /// <param name="value">
        /// The value of the http header.
        /// </param>
        [ContractVerification(true)]
        public FluentHttpHeader(string name, string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(!string.IsNullOrEmpty(Name));

            Name = name;
            Value = value;
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
        private void InvariantObject()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Name));
        }
    }
}