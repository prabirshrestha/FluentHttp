namespace FluentHttp
{
    /// <summary>
    /// Represents a pair.
    /// </summary>
    public class Pair<TName, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pair{TName,TValue}"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public Pair(TName name, TValue value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public TName Name { get; set; }

        /// <summary>
        /// Gets or sets Value.
        /// </summary>
        public TValue Value { get; set; }
    }
}