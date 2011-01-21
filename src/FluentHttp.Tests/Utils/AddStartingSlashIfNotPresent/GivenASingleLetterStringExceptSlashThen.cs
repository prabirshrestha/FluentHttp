namespace FluentHttp.Tests.Utils.AddStartingSlashIfNotPresent
{
    using FluentHttp;
    using Xunit;
    using Xunit.Extensions;

    public class GivenASingleLetterStringExceptSlashThen
    {
        [Theory]
        [InlineData("a")]
        public void LengthShouldBe2(string input)
        {
            var result = Utils.AddStartingSlashInNotPresent(input);

            Assert.Equal(2, result.Length);
        }

        [Theory]
        [InlineData("a")]
        public void ResultShouldStartWithSlashFollowedByTheLetter(string input)
        {
            var expected = "/" + input;
            var result = Utils.AddStartingSlashInNotPresent(input);

            Assert.Equal(expected, result);
        }
    }
}