using System;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Settings;

namespace SFA.DAS.FAT.MockServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mock Server starting on http://localhost:5003");

            var settings = new FluentMockServerSettings
            {
                Port = 5003,
                Logger = new WireMockConsoleLogger()
            };
            
            var server = StandAloneApp.Start(settings);
            
            server.Given(Request.Create().WithPath("/trainingcourses")
                    .UsingGet()
            ).RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("courses.json"));

            server.Given(Request.Create().WithPath(new RegexMatcher("/trainingcourses/([1-9]*)"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course.json"));

            server.Given(Request.Create().WithPath(new RegexMatcher("/trainingcourses/([1-9]*)/providers"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-providers.json"));

            Console.WriteLine(("Press any key to stop the server"));
            Console.ReadKey();
        }
    }
}
