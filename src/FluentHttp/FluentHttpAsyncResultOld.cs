namespace FluentHttp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;

    /// <summary>
    /// Async Result for <see cref="FluentHttp"/>.
    /// </summary>
    public class FluentHttpAsyncResultOld : IAsyncResult
    {
        /// <summary>
        /// The callback.
        /// </summary>
        private readonly AsyncCallback callback;

        /// <summary>
        /// Wait handle for the async result.
        /// </summary>
        private readonly ManualResetEvent waitHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentHttpAsyncResultOld"/> class.
        /// </summary>
        /// <param name="callback">
        /// The callback.
        /// </param>
        /// <param name="state">
        /// The state.
        /// </param>
        public FluentHttpAsyncResultOld(AsyncCallback callback, object state)
        {
            this.callback = callback;
            this.AsyncState = state;
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
            get { return this.waitHandle.WaitOne(0, false); }
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
            get { return this.waitHandle; }
        }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <returns>
        /// A user-defined object that qualifies or contains information about an asynchronous operation.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules",
            "SA1623:PropertySummaryDocumentationMustMatchAccessors", Justification = "Reviewed. Suppression is OK here.")]
        public object AsyncState { get; protected internal set; }

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
        /// Gets or sets the <see cref="HttpRequestState"/>.
        /// </summary>
        internal HttpRequestState HttpRequestState { get; set; }

        /// <summary>
        /// Signal the asynchronous operation has completed.
        /// </summary>
        internal void Complete()
        {
            this.waitHandle.Set();

            if (this.HttpRequestState != null)
            {
                // don't set the http request state to null.
                // EndRequest method still needs to access it.
                this.HttpRequestState.Dispose();
            }

            if (this.callback != null)
                this.callback(this);
        }
    }
}