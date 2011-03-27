namespace FluentHttp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the Http Header for Fluent Http
    /// </summary>
    public class FluentHttpHeader : Pair<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpHeader"/> class.
        /// </summary>
        /// <param name="name">
        /// The header name.
        /// </param>
        /// <param name="value">
        /// The header value.
        /// </param>
        public FluentHttpHeader(string name, string value)
            : base(name, value)
        {
        }
    }

    /// <summary>
    /// Collection of http headers.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class FluentHttpHeaders
    {
        /// <summary>
        /// Internal header collection.
        /// </summary>
        private readonly List<FluentHttpHeader> _httpHeaders = new List<FluentHttpHeader>();

        /// <summary>
        /// Adds the specified <see cref="FluentHttpHeader"/>.
        /// </summary>
        /// <param name="httpHeader">
        /// The http header.
        /// </param>
        /// <returns>
        /// The fluent http headers.
        /// </returns>
        public FluentHttpHeaders Add(FluentHttpHeader httpHeader)
        {
            _httpHeaders.Add(httpHeader);
            return this;
        }

        /// <summary>
        /// Adds the http header with the specified name and value.
        /// </summary>
        /// <param name="name">
        /// The http header name.
        /// </param>
        /// <param name="value">
        /// The http header value.
        /// </param>
        /// <returns>
        /// The fluent http headers.
        /// </returns>
        public FluentHttpHeaders Add(string name, string value)
        {
            return Add(new FluentHttpHeader(name, value));
        }

        /// <summary>
        /// Gets the internal header collection.
        /// </summary>
        /// <returns>
        /// The list of http header.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<FluentHttpHeader> GetHeaderCollection()
        {
            return _httpHeaders;
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