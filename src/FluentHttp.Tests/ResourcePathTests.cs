namespace FluentHttp.Tests
{
    using Xunit;
    using Xunit.Extensions;

    public class ResourcePathTests
    {
        [Theory]
        [InlineData("/me", "/me")]
        [InlineData("me", "/me")]
        [InlineData("", "")]
        [InlineData("m", "/m")]
        [InlineData(null, null)]
        public void SetResourcePath_AutoAppendSlashIfNeeded_Test(string input, string excepted)
        {
            var request = new FluentHttpRequestOld("https://graph.facebook.com")
                .ResourcePath(input);

            Assert.Equal(excepted, request.GetResourcePath());
        }
    }
}