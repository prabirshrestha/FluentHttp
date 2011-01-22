namespace FluentHttp.Tests
{
    using System.Net;
    using Xunit;

    public class CredentialsTests
    {
        [Fact]
        public void SetCredentialsTest()
        {
            var credentials = CredentialCache.DefaultCredentials;

            var request = new FluentHttpRequestOld("http://prabir.me")
                .Credentials(credentials);

            Assert.Equal(credentials, request.GetCredentials());
        }
    }
}