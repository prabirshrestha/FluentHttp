namespace FluentHttp.Tests.HttpHeader
{
    using System;
    using TechTalk.SpecFlow;
    using Xunit;

    [Binding]
    [StepScope(Feature = "FluentHttpHeader ctor")]
    public class HttpHeaderCtor
    {
        private FluentHttpHeader _fluentHttpHeader;
        private string _headerName;
        private string _headerValue;


        [Given(@"a null fluent http header")]
        public void GivenANullFluentHttpHeader()
        {
            _fluentHttpHeader = null;
        }

        [Then(@"name should be ""(.*)""")]
        public void ThenNameShouldBeHeader_Name(string headerName)
        {
            Assert.Equal(headerName, _headerName);
        }

        [Then(@"value should be ""(.*)""")]
        public void ThenValueShouldBeHeader_Value(string headerValue)
        {
            Assert.Equal(headerValue, _headerValue);
        }

        [When(@"I create a new fluent http header with ctor params \(""(.*)"" and ""(.*)""\)")]
        public void WhenICreateANewFluentHttpHeaderWithCtorParamsHeader_NameAndHeader_Value(string headerName, string headerValue)
        {
            _fluentHttpHeader = new FluentHttpHeader(headerName, headerValue);
        }

        [When(@"I get name")]
        public void WhenIGetName()
        {
            _headerName = _fluentHttpHeader.Name;
        }

        [When(@"I get value")]
        public void WhenIGetValue()
        {
            _headerValue = _fluentHttpHeader.Value;
        }

#if AGGRESSIVE_CHECK
        private Exception _exception;

        [Then(@"it should throw ArgumentOutOfRangeException")]
        public void ThenItShouldThrowArgumentNullException()
        {
            Assert.Equal(typeof(ArgumentOutOfRangeException), _exception.GetType());
        }

        [When(@"I create a new fluent http header with http header name as null")]
        public void WhenICreateANewFluentHttpHeaderWithHttpHeaderAsNull()
        {
            try
            {
                _fluentHttpHeader = new FluentHttpHeader(null, "header-value");
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I create a new fluent http header with http header name as string\.Empty")]
        public void WhenICreateANewFluentHttpHeaderWithHttpHeaderAsString_Empty()
        {
            try
            {
                _fluentHttpHeader = new FluentHttpHeader(null, "header-value");
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I create a new fluent http header with http header name as """"")]
        public void WhenICreateANewFluentHttpHeaderWithHttpHeaderAs()
        {
            try
            {
                _fluentHttpHeader = new FluentHttpHeader("", "header-value");
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [When(@"I create a new fluent http header with http header name as "" """)]
        public void WhenICreateANewFluentHttpHeaderWithHttpHeaderNameAs()
        {
            try
            {
                _fluentHttpHeader = new FluentHttpHeader(" ", "header-value");
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

#endif

    }
}
