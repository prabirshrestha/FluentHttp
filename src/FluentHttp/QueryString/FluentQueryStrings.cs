namespace FluentHttp
{
    using System.Collections;
    using System.Collections.Generic;

    public class FluentQueryStrings : IEnumerable<FluentQueryString>
    {
        private readonly List<FluentQueryString> _queryStrings = new List<FluentQueryString>();

        public FluentQueryStrings Add(FluentQueryString queryString)
        {
            _queryStrings.Add(queryString);
            return this;
        }

        public FluentQueryStrings Add(string name, string value)
        {
            _queryStrings.Add(new FluentQueryString(name, value));
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
        public IEnumerator<FluentQueryString> GetEnumerator()
        {
            return _queryStrings.GetEnumerator();
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