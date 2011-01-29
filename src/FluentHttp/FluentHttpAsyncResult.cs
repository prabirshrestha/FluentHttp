namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Threading;

    /// <summary>
    /// Represents the intnerla fluent http async result.
    /// </summary>
    internal class FluentHttpAsyncResult : IAsyncResult
    {
        /// <summary>
        /// Internal state.
        /// </summary>
        private readonly InternalState internalState;

        /// <summary>
        /// The wait handle.
        /// </summary>
        private readonly ManualResetEvent waitHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpAsyncResult"/> class.
        /// </summary>
        /// <param name="internalState">
        /// The internal state.
        /// </param>
        internal FluentHttpAsyncResult(InternalState internalState)
        {
            Contract.Requires(internalState != null);

            this.internalState = internalState;
            this.waitHandle = new ManualResetEvent(false);
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation has completed.
        /// </summary>
        /// <returns>
        /// true if the operation is complete; otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public bool IsCompleted
        {
            get
            {
#if SILVERLIGHT
                return this.waitHandle.WaitOne(0);
#else
                return this.waitHandle.WaitOne(0, false);
#endif
            }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Threading.WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                Contract.Ensures(Contract.Result<WaitHandle>() != null);
                return this.waitHandle;
            }
        }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A user-defined object that qualifies or contains information about an asynchronous operation.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public object AsyncState
        {
            get { return this.InternalState.State; }
        }

        /// <summary>
        /// Gets a value indicating whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <returns>
        /// true if the asynchronous operation completed synchronously; otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public bool CompletedSynchronously
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the internal state.
        /// </summary>
        internal InternalState InternalState
        {
            get { return internalState; }
        }

        /// <summary>
        /// Set status as completed.
        /// </summary>
        internal void Complete()
        {
            this.waitHandle.Set();

            if (this.InternalState != null)
            {
                if (this.internalState.Callback != null)
                {
                    this.internalState.Callback(this);
                }
            }
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
          Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        private void InvarianObject()
        {
            Contract.Invariant(this.internalState != null);
            Contract.Invariant(this.waitHandle != null);
        }
    }
}