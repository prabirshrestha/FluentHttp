namespace FluentHttp.Tests.Utils.AddStartingSlashIfNotPresent
{
    using FluentHttp;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAStringWithoutSlashThen
    {
        [Theory]
        [InlineData("path")]
        public void ItShouldAddSlashAtTheBeginning(string input)
        {
            var result = Utils.AddStartingSlashInNotPresent(input);

            Assert.Equal('/', result[0]);
        }

        [Fact]
        public void LengthOfResultShouldBe1MoreThanTheOriginalInput()
        {
            var input = "dummy";
            var result = Utils.AddStartingSlashInNotPresent(input);

            Assert.Equal(input.Length + 1, result.Length);
        }

        [Fact]
        public void ResultShouldBeSlashConcatentatedWithOriginalInput()
        {
            var input = "dummy";
            var result = Utils.AddStartingSlashInNotPresent(input);

            Assert.Equal("/" + input, result);
        }
    }
}