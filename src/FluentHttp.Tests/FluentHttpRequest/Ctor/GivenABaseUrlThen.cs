namespace FluentHttp.Tests.FluentHttpRequest.Ctor
{
    using FluentHttp;
    using Xunit;

    public class GivenABaseUrlThen
    {
        private FluentHttpRequest request;

        public GivenABaseUrlThen()
        {
            request = new FluentHttpRequest("https://graph.facebook.com");
        }

        [Fact]
        public void WhenGetBaseUrlShouldNotBeNullOrEmpty()
        {
            var baseUrl = this.request.BaseUrl;

            Assert.False(string.IsNullOrEmpty(baseUrl));
        }

        [Fact]
        public void BaseUrlShouldBeSameAsOringialBaseUrl()
        {
            var originalBaseUrl = "https://graph.facebook.com";
            var request = new FluentHttpRequest(originalBaseUrl);

            Assert.Equal(originalBaseUrl, request.BaseUrl);
        }

        [Fact]
        public void WhenGetTheMethodShouldBeGet()
        {
            var method = this.request.GetMethod();

            Assert.Equal("GET", method);
        }

        [Fact]
        public void WhenIGentTheBufferSizeItShouldBe4096()
        {
            var bufferSize = this.request.GetBufferSize();

            Assert.Equal(4096, bufferSize);
        }
    }
}