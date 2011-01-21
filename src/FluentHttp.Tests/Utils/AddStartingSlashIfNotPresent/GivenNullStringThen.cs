namespace FluentHttp.Tests.Utils.AddStartingSlashIfNotPresent
{
    using Xunit;

    public class GivenNullStringThen
    {
        [Fact]
        public void ResultShouldBeSlash()
        {
            var result = FluentHttp.Utils.AddStartingSlashInNotPresent(null);

            Assert.Equal("/", result);
        }
    }
}