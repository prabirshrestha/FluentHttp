namespace FluentHttp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the query string for Fluent Http
    /// </summary>
    public class FluentQueryString : Pair<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentQueryString"/> class.
        /// </summary>
        /// <param name="name">
        /// The header name.
        /// </param>
        /// <param name="value">
        /// The header value.
        /// </param>
        public FluentQueryString(string name, string value)
            : base(name, value)
        {
        }
    }

    /// <summary>
    /// Collection of query strings.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class FluentQueryStrings
    {
        /// <summary>
        /// Internal query string collection.
        /// </summary>
        private readonly List<FluentQueryString> _queryStrings = new List<FluentQueryString>();

        /// <summary>
        /// Adds the specified <see cref="FluentQueryString"/>.
        /// </summary>
        /// <param name="queryString">
        /// The query string.
        /// </param>
        /// <returns>
        /// The fluent query strings.
        /// </returns>
        public FluentQueryStrings Add(FluentQueryString queryString)
        {
            _queryStrings.Add(queryString);
            return this;
        }

        /// <summary>
        /// Adds the query string with the specified name and value.
        /// </summary>
        /// <param name="name">
        /// The http query string name.
        /// </param>
        /// <param name="value">
        /// The http query string value.
        /// </param>
        /// <returns>
        /// The fluent query string.
        /// </returns>
        public FluentQueryStrings Add(string name, string value)
        {
            return Add(new FluentQueryString(name, value));
        }

        /// <summary>
        /// Gets the internal query string collection.
        /// </summary>
        /// <returns>
        /// The list of query strings.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<FluentQueryString> GetQueryStringCollection()
        {
            return _queryStrings;
        }

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
    }
}