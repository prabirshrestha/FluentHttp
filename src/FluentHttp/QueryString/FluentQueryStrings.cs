using System;
using System.ComponentModel;

namespace FluentHttp
{
    using System.Collections;
    using System.Collections.Generic;

    public class FluentQueryStrings : IEnumerable<FluentQueryString>
    {
        private readonly List<FluentQueryString> _queryStrings = new List<FluentQueryString>();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentQueryStrings Add(FluentQueryString queryString)
        {
            _queryStrings.Add(queryString);
            return this;
        }

        public FluentQueryStrings Add(string name, string value)
        {
            return Add(name, value, true);
        }

        public FluentQueryStrings Add(string name,string value,bool encode)
        {
            _queryStrings.Add(new FluentQueryString(name, value, encode));
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
        [EditorBrowsable(EditorBrowsableState.Never)]        
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

        #region Hide defualt object methods

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

#pragma warning disable 0108
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Type GetType()
        {
            return base.GetType();
        }
#pragma warning restore 0108

        #endregion
    }
}