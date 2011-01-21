namespace FluentHttp
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Fluent Http Request Body
    /// </summary>
    public class FluentHttpRequestBody
    {
        private const string NewLine = "\r\n";
        private readonly MultiReadStream multiReadStream;

        // http://www.briangrinstead.com/blog/multipart-form-post-in-c
        // make it const string for performance
        string multipartFormDataBoundary = "28947758029299";
        private string prefix = "--";

        // string MultipartFormContentType = "multipart/form-data; boundary=" + MultipartFormDataBoundary;

        private bool isMultipartFormData;

        public FluentHttpRequestBody()
        {
            this.multiReadStream = new MultiReadStream();
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody MultipartFormDataBoundary(string boundary)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            this.multipartFormDataBoundary = boundary;

            return this;
        }

        public string GetMultipartFormDataBoundary()
        {
            return this.multipartFormDataBoundary;
        }

        /// <summary>
        /// Appends the stream
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// </returns>
        [ContractVerification(true)]
        public FluentHttpRequestBody Append(Stream stream)
        {
            Contract.Requires(stream != null);
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            this.multiReadStream.AddStream(stream);

            return this;
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody Append(byte[] data)
        {
            Contract.Requires(data != null);
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            return this.Append(new MemoryStream(data));
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody Append(string contents, Encoding encoding)
        {
            Contract.Requires(!string.IsNullOrEmpty(contents));
            Contract.Requires(encoding != null);
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            return Append(encoding.GetBytes(contents));
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody Append(string contents)
        {
            Contract.Requires(!string.IsNullOrEmpty(contents));
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            return Append(contents, Encoding.UTF8);
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody AppendLine()
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            return Append(NewLine);
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody AppendLine(string contents)
        {
            Contract.Requires(!string.IsNullOrEmpty(contents));
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            var b = Append(contents, Encoding.UTF8);
            return b.AppendLine();
        }

        [ContractVerification(true)]
        public FluentHttpRequestBody AppendFormat(string format, params object[] args)
        {
            Contract.Requires(!string.IsNullOrEmpty(format));
            Contract.Requires(args != null);
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            var str = string.Format(format, args);
            Contract.Assume(!string.IsNullOrEmpty(str));

            return Append(str, Encoding.UTF8);
        }

        //[ContractVerification(true)]
        public FluentHttpRequestBody Append(IDictionary<string, object> parameters)
        {
            Contract.Ensures(Contract.Result<FluentHttpRequestBody>() != null);

            if (parameters == null || parameters.Count == 0)
            {
                return this;
            }

            var sb = new StringBuilder();

            foreach (var parameter in parameters)
            {
                sb.AppendFormat("{0}={1}&", Utils.UrlEncode(parameter.Key), Utils.UrlEncode(parameter.Value.ToString()));
            }

            // remove the last &
            --sb.Length;

            return Append(sb.ToString());
        }

        public FluentHttpRequestBody MultipartSeperator()
        {
            this.isMultipartFormData = true;
            return Append(this.prefix + this.GetMultipartFormDataBoundary() + NewLine);
        }

        public FluentHttpRequestBody EndMultipart()
        {
            this.isMultipartFormData = true;
            return Append(this.prefix + this.GetMultipartFormDataBoundary() + this.prefix + NewLine);
        }

        public bool IsMultipartFormData()
        {
            return this.isMultipartFormData;
        }

        /*
        private FluentHttpRequestBody AppendFile(string path, IDictionary<string, object> parameters)
        {
            return this;
        }

        private FluentHttpRequestBody AppendFile(byte[] data, IDictionary<string, object> parameters)
        {
            return this;
        }

        private FluentHttpRequestBody AppendFile(Stream stream, IDictionary<string, object> parameters)
        {
            return this;
        }

        public FluentHttpRequestBody AppendFile(string path)
        {
            return AppendFile(path, (IDictionary<string, object>)null);
        }

        public FluentHttpRequestBody AppendFile(string path, string fileName)
        {
            return this;
        }

        public FluentHttpRequestBody AppendFile(string path, string fileName, string contentType)
        {
            return this;
        }

        public FluentHttpRequestBody AppendFile(byte[] data, string fileName)
        {
            return this;
        }

        public FluentHttpRequestBody AppendFile(byte[] data, string fileName, string contentType)
        {
            return this;
        }

        public FluentHttpRequestBody AppendFile(Stream stream, string fileName)
        {
            return this;
        }

        public FluentHttpRequestBody AppendFile(Stream stream, string fileName, string contentType)
        {
            return this;
        }
        */

        public Stream GetStream()
        {
            return this.multiReadStream;
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
            Justification = "Reviewed. Suppression is OK here."), ContractInvariantMethod]
        [ContractVerification(true)]
        private void InvariantObject()
        {
            Contract.Invariant(this.multiReadStream != null);
        }
    }
}