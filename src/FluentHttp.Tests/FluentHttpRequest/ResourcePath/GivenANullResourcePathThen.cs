namespace FluentHttp.Tests.FluentHttpRequest.ResourcePath
{
    using FluentHttp;
    using Xunit;

    public class GivenANullResourcePathThen
    {
        private readonly IFluentHttpRequest request;

        public GivenANullResourcePathThen()
        {
            this.request = new FluentHttpRequest("https://graph.facebook.com")
                               .ResourcePath(null);
        }

        [Fact]
        public void ResourcePathShouldBeNull()
        {
            var result = this.request.GetResourcePath();

            Assert.NotNull(result);
        }

        [Fact]
        public void ResourcePathShouldBeEmptyString()
        {
            var result = this.request.GetResourcePath();

            Assert.Equal(string.Empty, result);
        }
    }
}