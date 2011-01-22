namespace FluentHttp.Tests.FluentHttpRequest.ResourcePath
{
    using System.Collections.Generic;
    using FluentHttp;
    using Xunit;
    using Xunit.Extensions;

    public class GivenANonNullOrNonEmptyResourcePathStartingWihoutSlashThen
    {
        [PropertyData("ResourcePathsOnly")]
        [Theory]
        public void ItShouldStartWithSlash(string originalResourcePath)
        {
            var request = new FluentHttpRequest("https://www.facebook.com")
                              .ResourcePath(originalResourcePath);

            var resourcePath = request.GetResourcePath();

            Assert.True(resourcePath.StartsWith("/"));
        }

        [PropertyData("ResourcePathsOnly")]
        [Theory]
        public void ItShouldStartWithSlashFollowedByTheOriginalResourcePath(string originalResourcePath)
        {
            var expcetedResourcePath = string.Concat("/", originalResourcePath);

            var request = new FluentHttpRequest("https://www.facebook.com")
                              .ResourcePath(originalResourcePath);

            var resourcePath = request.GetResourcePath();

            Assert.Equal(expcetedResourcePath, resourcePath);
        }

        public static IEnumerable<object[]> ResourcePathsOnly
        {
            get
            {
                yield return new object[] { "me" };
                yield return new object[] { "c" };
                yield return new object[] { "1" };
                yield return new object[] { "me/picture" };
                yield return new object[] { "me/picture/" };
            }
        }
    }
}