namespace FluentHttp
{
    using System;
    using System.ComponentModel;

    public partial class FluentHttpRequest
    {
        private FluentQueryStrings _fluentQueryStrings;
        private FluentQueryStrings HttpQueryStrings
        {
            get { return _fluentQueryStrings ?? (_fluentQueryStrings = new FluentQueryStrings()); }
        }

        public FluentHttpRequest QueryStrings(Action<FluentQueryStrings> queryStrings)
        {
            if (queryStrings != null)
                queryStrings(HttpQueryStrings);
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public FluentQueryStrings GetQueryStrings()
        {
            return HttpQueryStrings;
        }
    }
}