
namespace FluentHttp.Tests.FluentHttpRequest.ResourcePath
{
    using FluentHttp;
    using Xunit;

    public class GivenAnEmptyResourcePathThen
    {
        private readonly IFluentHttpRequest request;

        public GivenAnEmptyResourcePathThen()
        {
            this.request = new FluentHttpRequest("https://graph.facebook.com")
                               .ResourcePath(string.Empty);
        }

        [Fact]
        public void ResourcePathShouldBeEmptyString()
        {
            var result = this.request.GetResourcePath();

            Assert.Equal(string.Empty, result);
        }
    }
}