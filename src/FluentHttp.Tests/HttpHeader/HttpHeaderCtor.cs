namespace FluentHttp.Tests.HttpHeader
{
    using TechTalk.SpecFlow;
    using Xunit;

    [Binding]
    public class HttpHeaderCtor
    {
        private FluentHttpHeader fluentHttpHeader;
        private string name;
        private string value;

        [Given(@"a new fluent http header with ctor params \(""(.*)"" and ""(.*)""\)")]
        public void GivenANewFluentHttpHeaderWithCtorParamsHeader_NameAndHeader_Value(string headerName, string headerValue)
        {
            fluentHttpHeader = new FluentHttpHeader(headerName, headerValue);
        }

        [When(@"I get name")]
        public void WhenIGetName()
        {
            name = fluentHttpHeader.Name;
        }

        [When(@"I get value")]
        public void WhenIGetValue()
        {
            value = fluentHttpHeader.Value;
        }

        [Then(@"name should be ""(.*)""")]
        public void ThenNameShouldBeHeader_Name(string headerName)
        {
            Assert.Equal(headerName, name);
        }

        [Then(@"value should be ""(.*)""")]
        public void ThenValueShouldBeHeader_Value(string headerValue)
        {
            Assert.Equal(headerValue, value);
        }
    }
}
