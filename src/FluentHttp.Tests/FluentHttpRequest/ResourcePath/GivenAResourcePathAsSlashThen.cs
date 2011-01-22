namespace FluentHttp.Tests.FluentHttpRequest.ResourcePath
{
    using FluentHttp;
    using Xunit;

    public class GivenAResourcePathAsSlashThen
    {
        [Fact]
        public void ResourcePathShouldBeSlash()
        {
            var request = new FluentHttpRequest("https://graph.facebook.com").ResourcePath("/");

            var resourcePath = request.GetResourcePath();

            Assert.Equal("/", resourcePath);
        }
    }
}