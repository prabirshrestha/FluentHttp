namespace FluentHttp.Tests.FluentHttpRequest.Timeout
{
    using FluentHttp;
    using Xunit;

    public class WhenGivenANewRequestAndISetTimeoutThen
    {
        [Fact]
        public void ItShouldSetTheTimeout()
        {
            var request = new FluentHttpRequest("https://graph.facebook.com");

            request.Timeout(100);

            var timeout = request.GetTimeout();

            Assert.Equal(100, timeout);
        }
    }
}