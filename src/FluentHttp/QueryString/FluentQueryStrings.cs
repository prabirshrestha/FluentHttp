namespace FluentHttp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Fluent QueryString
    /// </summary>
    public class FluentQueryStrings : IEnumerable<FluentQueryString>
    {
        /// <summary>
        /// List of query strings.
        /// </summary>
        private readonly List<FluentQueryString> queryStrings = new List<FluentQueryString>();

        /// <summary>
        /// Add a querystring
        /// </summary>
        /// <param name="queryString">
        /// The query string to add.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentQueryStrings"/>.
        /// </returns>
        [ContractVerification(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentQueryStrings Add(FluentQueryString queryString)
        {
            Contract.Requires(queryString != null);
            Contract.Ensures(Contract.Result<FluentQueryStrings>() != null);

            this.queryStrings.Add(queryString);

            return this;
        }

        /// <summary>
        /// Add a querystring.
        /// </summary>
        /// <param name="name">
        /// The name of the querystring.
        /// </param>
        /// <param name="value">
        /// The value of the querystring.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentQueryStrings"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentQueryStrings Add(string name, string value)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<FluentQueryStrings>() != null);

            return Add(name, value, true);
        }

        /// <summary>
        /// Add a querystring.
        /// </summary>
        /// <param name="name">
        /// The name of the querystring.
        /// </param>
        /// <param name="value">
        /// The value of the querystring.
        /// </param>
        /// <param name="encode">
        /// Indicate whether to encode the querystring.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentQueryStrings"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentQueryStrings Add(string name, string value, bool encode)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(Contract.Result<FluentQueryStrings>() != null);

            this.queryStrings.Add(new FluentQueryString(name, value, encode));

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
            return queryStrings.GetEnumerator();
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

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [EditorBrowsable(EditorBrowsableState.Never)]
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
            Contract.Invariant(this.queryStrings != null);
        }
    }
}