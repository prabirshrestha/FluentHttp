namespace FluentHttp
{
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
        public FluentQueryString(string name, string value)
        {
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