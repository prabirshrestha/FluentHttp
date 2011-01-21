namespace FluentHttp.Tests.Utils.AddStartingSlashIfNotPresent
{
    using Xunit;
    using Xunit.Extensions;

    public class GivenAStringStartingWithSlashThen
    {
        [Theory]
        [InlineData("/test")]
        [InlineData("/testing")]
        public void ResultShouldBeSameAsInput(string input)
        {
            var result = FluentHttp.Utils.AddStartingSlashInNotPresent(input);

            Assert.Equal(result, input);
        }
    }
}