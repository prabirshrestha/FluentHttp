namespace FluentHttp.Tests.FluentHttpRequest.ResourcePath
{
    using FluentHttp;
    using Xunit;

    public class GivenARequestWithResourcePathAlreadySetWhenISetANewResourcePathAsNullThen
    {
        [Fact]
        public void ItResourcePathShouldBeEmptyString()
        {
            var request = new FluentHttpRequest("https://graph.facebook.com").ResourcePath("/me");

            request.ResourcePath(null);

            var result = request.GetResourcePath();

            Assert.Equal(string.Empty, result);
        }
    }
}