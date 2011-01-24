namespace FluentHttp
{
    using System;

    /// <summary>
    /// Event Args for ExecutingEventArgs
    /// </summary>
    public class ExecutingEventArgs : EventArgs
    {
        /// <summary>
        /// Fluent Http Request.
        /// </summary>
        private readonly IFluentHttpRequest fluentHttpRequest;

        /// <summary>
        /// User state.
        /// </summary>
        private readonly object userState;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutingEventArgs"/> class.
        /// </summary>
        /// <param name="fluentHttpRequest">
        /// The fluent http request.
        /// </param>
        /// <param name="userState">
        /// The user state.
        /// </param>
        public ExecutingEventArgs(IFluentHttpRequest fluentHttpRequest, object userState)
        {
            this.fluentHttpRequest = fluentHttpRequest;
            this.userState = userState;
        }

        /// <summary>
        /// Gets the user state.
        /// </summary>
        public object UserState
        {
            get { return this.userState; }
        }

        /// <summary>
        /// Gets the <see cref="FluentHttpRequest"/>.
        /// </summary>
        public IFluentHttpRequest FluentHttpRequest
        {
            get { return this.fluentHttpRequest; }
        }
    }
}