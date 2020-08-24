using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Courses.Api;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.MockServer;
using TechTalk.SpecFlow;
using WireMock.Server;

namespace SFA.DAS.FAT.Web.AcceptanceTests.Infrastructure
{
    [Binding]
    public sealed class TestEnvironmentManagement
    {
        private readonly ScenarioContext _context;
        private static HttpClient _staticClient;
        private static IWireMockServer _staticServer;
        private Mock<IApiClient> _mockApiClient;

        public TestEnvironmentManagement(ScenarioContext context)
        {
            _context = context;
        }

        [BeforeScenario("WireMockServer")]
        public void StartWebApp()
        {
            _staticServer = MockApiServer.Start();
            _staticClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(options=>options.Build())
                .CreateClient();
            _context.Set(_staticClient,ContextKeys.HttpClient);
        }

        [BeforeScenario("MockApiClient")]
        public void StartWebAppWithMockApiClient()
        {
            _mockApiClient = new Mock<IApiClient>();

            _mockApiClient.Setup(x => x.Get<TrainingCourses>(It.IsAny<GetCoursesApiRequest>()))
                .ReturnsAsync(new TrainingCourses());
            
            var testWebApplicationFactory = new WebApplicationFactory<Startup>();
            testWebApplicationFactory.WithWebHostBuilder(options =>
            {
                options.Build();
            });
            
            _staticClient = new CustomWebApplicationFactory<Startup>(_mockApiClient).CreateClient();
            
            _context.Set(_staticClient,ContextKeys.HttpClient);
            _context.Set(_mockApiClient,ContextKeys.MockApiClient);
        }

        [AfterScenario]
        public static void StopEnvironment()
        {
            _staticServer.Stop();
        }
    }

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly Mock<IApiClient> _mockApiClient;

        public CustomWebApplicationFactory(Mock<IApiClient> mockApiClient)
        {
            _mockApiClient = mockApiClient;
        }

        
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(IApiClient));

                services.Remove(descriptor);

                services.AddSingleton(_mockApiClient.Object);

                services.BuildServiceProvider();

            });
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseTestServer();
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(IApiClient));

                services.Remove(descriptor);

                services.AddSingleton(_mockApiClient.Object);

                services.BuildServiceProvider();

            });
        }
    }
}
