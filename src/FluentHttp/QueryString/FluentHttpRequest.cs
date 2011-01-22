using System.Collections.Generic;

namespace FluentHttp
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1601:PartialElementsMustBeDocumented",
        Justification = "Reviewed. Suppression is OK here.")]
    public partial class FluentHttpRequestOld
    {
        /// <summary>
        /// Internal list of http querystrings.
        /// </summary>
        private FluentQueryStrings httpQueryStrings;

        /// <summary>
        /// Gets the http querystrings.
        /// </summary>
        [ContractVerification(true)]
        private FluentQueryStrings HttpQueryStrings
        {
            get
            {
                Contract.Ensures(Contract.Result<FluentQueryStrings>() != null);

                return this.httpQueryStrings ?? (this.httpQueryStrings = new FluentQueryStrings());
            }
        }

        /// <summary>
        /// Access querystrings.
        /// </summary>
        /// <param name="queryStrings">
        /// The querystrings.
        /// </param>
        /// <returns>
        /// Returns <see cref="FluentHttpRequestOld"/>.
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld QueryStrings(Action<FluentQueryStrings> queryStrings)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            if (queryStrings != null)
                queryStrings(this.HttpQueryStrings);

            return this;
        }

        /// <summary>
        /// Adds the specified querystrings.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="encode">
        /// The encode.
        /// </param>
        /// <returns>
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld QueryStrings(IDictionary<string, object> parameters, bool encode)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            this.GetQueryStrings().Add(parameters, encode);

            return this;
        }

        /// <summary>
        /// Adds the specified querystrings.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestOld QueryStrings(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestOld>() != null);

            return this.QueryStrings(parameters, true);
        }

        /// <summary>
        /// Gets the querystrings.
        /// </summary>
        /// <returns>
        /// Returns <see cref="FluentQueryStrings"/>.
        /// </returns>
        [ContractVerification(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public FluentQueryStrings GetQueryStrings()
        {
            Contract.Ensures(Contract.Result<FluentQueryStrings>() != null);

            return this.HttpQueryStrings;
        }
    }
}