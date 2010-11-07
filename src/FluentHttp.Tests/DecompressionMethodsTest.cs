namespace FluentHttp.Tests
{
    using System.Net;
    using Xunit;

    public class DecompressionMethodsTest
    {
        [Fact]
        public void DefaultDecompressionMethod_ContainsAllDecompressionMethods_Test()
        {
            var request = new FluentHttpRequest("http://prabir.me");

            var decompressionMethods = request.GetDecompressionMethods();

            Assert.Equal(DecompressionMethods.None, decompressionMethods & DecompressionMethods.None);
            Assert.Equal(DecompressionMethods.Deflate, decompressionMethods & DecompressionMethods.Deflate);
            Assert.Equal(DecompressionMethods.GZip, decompressionMethods & DecompressionMethods.GZip);
        }

        [Fact]
        public void SetDecompressionMethodTest()
        {
            var request = new FluentHttpRequest("http://prabir.me")
                .DecompressionMethods(DecompressionMethods.Deflate | DecompressionMethods.None);

            var decompressionMethods = request.GetDecompressionMethods();

            Assert.Equal(DecompressionMethods.None, decompressionMethods & DecompressionMethods.None);
            Assert.Equal(DecompressionMethods.Deflate, decompressionMethods & DecompressionMethods.Deflate);
            Assert.NotEqual(DecompressionMethods.GZip, decompressionMethods & DecompressionMethods.GZip);

        }
    }
}