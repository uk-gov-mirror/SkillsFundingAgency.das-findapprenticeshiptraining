using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using SFA.DAS.FAT.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAT.Web.AcceptanceTests.Steps
{
    [Binding]
    public sealed class HttpSteps
    {
        private readonly ScenarioContext _context;

        public HttpSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given("I have an http client")]
        public void GivenIHaveAnHttpClient()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();
            _context.Set(client,ContextKeys.HttpClient);
        }

        [When("I navigate to the following url: (.*)")]
        public async Task WhenINavigateToTheFollowingUrl(string url)
        {
            var client = _context.Get<HttpClient>(ContextKeys.HttpClient);
            var response = await client.GetAsync(url);
            _context.Set(response, ContextKeys.HttpResponse);
        }

        [Then("an http status code of (.*) is returned")]
        public void ThenAnHttpStatusCodeIsReturned(int httpStatusCode)
        {
            if (!_context.TryGetValue<HttpResponseMessage>(ContextKeys.HttpResponse, out var result))
            {
                Assert.Fail($"scenario context does not contain value for key [{ContextKeys.HttpResponse}]");
            }

            result.StatusCode.Should().Be(httpStatusCode);
        }
    }
}