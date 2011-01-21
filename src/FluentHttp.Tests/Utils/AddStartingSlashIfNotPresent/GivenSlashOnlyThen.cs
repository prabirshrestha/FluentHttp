namespace FluentHttp.Tests.Utils.AddStartingSlashIfNotPresent
{
    using Xunit;

    public class GivenSlashOnlyThen
    {
        [Fact]
        public void ResultShouldBeSlash()
        {
            var result = FluentHttp.Utils.AddStartingSlashInNotPresent("/");

            Assert.Equal("/", result);
        }
    }
}