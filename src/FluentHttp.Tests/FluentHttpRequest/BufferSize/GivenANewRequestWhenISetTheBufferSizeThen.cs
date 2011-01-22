namespace FluentHttp.Tests.FluentHttpRequest.BufferSize
{
    using FluentHttp;
    using Xunit;

    public class GivenANewRequestWhenISetTheBufferSizeThen
    {
        [Fact]
        public void ItShouldSetItCorrectly()
        {
            var request = new FluentHttpRequest("https://graph.facebook.com");

            request.BufferSize(10);
            var bufferSize = request.GetBufferSize();

            Assert.Equal(10, bufferSize);
        }
    }
}