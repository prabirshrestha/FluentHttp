namespace FluentHttp.Tests.QueryString
{
    using TechTalk.SpecFlow;
    using Xunit;

    [Binding]
    [StepScope(Feature = "FluentQueryString ctor")]
    public class QueryStringCtor
    {
        private FluentQueryString _fluentQueryString;
        private string _qsName;
        private string _qsValue;

        [Given(@"a null fluent querystring header")]
        public void GivenANullFluentQuerystringHeader()
        {
            _fluentQueryString = null;
        }

        [When(@"I create a new fluent querystring with ctor params \(""(.*)"" and ""(.*)""\)")]
        public void WhenICreateANewFluentQuerystringWithCtorParamsQs_NameAndQs_Value(string qsName, string qsValue)
        {
            _fluentQueryString = new FluentQueryString(qsName, "qs-value");
        }

        [When(@"I get name")]
        public void WhenIGetName()
        {
            _qsName = _fluentQueryString.Name;
        }

        [When(@"I get value")]
        public void WhenIGetValue()
        {
            _qsValue = _fluentQueryString.Value;
        }

        [Then(@"name should be ""(.*)""")]
        public void ThenNameShouldBeQsName(string qsName)
        {
            Assert.Equal(_qsName, qsName);
        }

        [Then(@"value should be ""(.*)""")]
        public void ThenValueShouldBeQsValue(string qsValue)
        {
            Assert.Equal(_qsValue, qsValue);
        }
    }
}
