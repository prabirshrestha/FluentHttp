namespace FluentHttp
{
    using System;

    /// <summary>
    /// Http Header for Fluent Http
    /// </summary>
    public class FluentHttpHeader : INameValue
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
        public FluentHttpHeader(string name, string value)
        {
#if AGGRESSIVE_CHECK
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
                throw new ArgumentNullException("name");
#endif
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
    }
}