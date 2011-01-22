namespace FluentHttp.Tests.FluentHttpRequest.Method
{
    using FluentHttp;
    using Xunit;
    using Xunit.Extensions;

    public class GivenAVaildMethodThen
    {
        [InlineData("GET")]
        [InlineData("GeT")]
        [InlineData("get")]
        [InlineData("POST")]
        [InlineData("post")]
        [Theory]
        public void ItShouldReturnTheOriginalMethod(string originalMethod)
        {
            var request = new FluentHttpRequest("https://graph.facebook.com").Method(originalMethod);

            var method = request.GetMethod();

            Assert.Equal(originalMethod, method);
        }
    }
}