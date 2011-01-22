using Xunit.Extensions;

namespace FluentHttp.Tests.FluentHttpRequest.ResourcePath
{
    using FluentHttp;
    using Xunit;

    public class GivenARequestWithResourcePathAlreadySetWhenISetANewResourcePathThen
    {
        private IFluentHttpRequest request;

        public GivenARequestWithResourcePathAlreadySetWhenISetANewResourcePathThen()
        {
            request = new FluentHttpRequest("https://graph.facebook.com").ResourcePath("/me");
        }

        [InlineData("")]
        [InlineData("/me/picture")]
        [Theory]
        public void ItShouldOverrideTheResourcePath(string resourcePath)
        {
            this.request.ResourcePath(resourcePath);

            var result = this.request.GetResourcePath();

            Assert.Equal(resourcePath, result);
        }
    }
}