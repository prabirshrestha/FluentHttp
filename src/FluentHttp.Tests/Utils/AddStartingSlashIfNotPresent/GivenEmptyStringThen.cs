namespace FluentHttp.Tests.Utils.AddStartingSlashIfNotPresent
{
    using Xunit;

    public class GivenEmptyStringThen
    {
        [Fact]
        public void ResultShouldBeSlash()
        {
            var result = FluentHttp.Utils.AddStartingSlashInNotPresent(string.Empty);
            
            Assert.Equal("/", result);
        }
    }
}