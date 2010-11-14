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
        private readonly FluentHttpRequest _fluentHttpRequest;

        private readonly object _userState;

        public ExecutingEventArgs(FluentHttpRequest fluentHttpRequest,object userState)
        {
            _fluentHttpRequest = fluentHttpRequest;
            _userState = userState;
        }

        public object UserState
        {
            get { return _userState; }
        }

        /// <summary>
        /// Gets the FluentHttpRequest.
        /// </summary>
        public FluentHttpRequest FluentHttpRequest
        {
            get { return _fluentHttpRequest; }
        }
    }
}