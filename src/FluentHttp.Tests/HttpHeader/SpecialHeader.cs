namespace FluentHttp.Tests.HttpHeader
{
    using Xunit;
    using Xunit.Extensions;

    public class SpecialHeaders
    {
        [Theory]
        [InlineData("accept")]
        [InlineData("connection")]
        [InlineData("content-length")]
        [InlineData("content-type")]
        [InlineData("expect")]
        [InlineData("date")]
        [InlineData("host")]
        [InlineData("if-modified-since")]
        [InlineData("range")]
        [InlineData("referer")]
        [InlineData("transfer-encoding")]
        [InlineData("user-agent")]
        public void IsSpecialHeaders_ForSpecialHeaders_Tests(string headerName)
        {
            var actual = FluentHttpHeaders.IsSpecialHeader(headerName);

            Assert.True(actual >= 0);
        }

        [Theory]
        [InlineData("aCcept")]
        [InlineData("connection")]
        [InlineData("Content-Length")]
        [InlineData("content-Type")]
        [InlineData("ExpEct")]
        public void IsSpecialHeaders_ForSpecialHeader_DiffernetCasingTest(string headerName)
        {
            var actual = FluentHttpHeaders.IsSpecialHeader(headerName);
            
            Assert.True(actual >= 0);
        }

        [Theory]
        [InlineData("header")]
        [InlineData("authorization")]
        public void IsSpecialHeaders_ForNonSpecialHeaders_Test(string headerName)
        {
            var actual = FluentHttpHeaders.IsSpecialHeader(headerName);

            Assert.Equal(-1, actual);
        }
    }
}