namespace FluentHttp.Tests
{
    using Xunit;
    using Xunit.Extensions;

    public class TimeoutTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        public void SetTimeOutTest(int timeout)
        {
            var request = new FluentHttpRequestOld("http://prabir.me")
                .Timeout(timeout);

            Assert.Equal(timeout, request.GetTimeout());
        }
    }
}