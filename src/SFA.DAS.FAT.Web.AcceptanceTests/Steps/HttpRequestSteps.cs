using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SFA.DAS.FAT.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAT.Web.AcceptanceTests.Steps
{
    [Binding]
    public sealed class HttpRequestSteps
    {
        private readonly ScenarioContext _context;

        public HttpRequestSteps(ScenarioContext context)
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
    }
}
