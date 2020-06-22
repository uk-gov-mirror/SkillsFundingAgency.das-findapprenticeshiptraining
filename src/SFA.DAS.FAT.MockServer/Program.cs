using System;
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
                Port = 5003
            };
            
            
            var server = StandAloneApp.Start(settings);
            
            server.Given(Request.Create().WithPath("/trainingcourses")
                    .UsingGet()
            ).RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "application/json")
                        .WithBodyFromFile("courses.json"));

            server.Given(Request.Create().WithPath("/trainingcourses/*")
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course.json"));

            
            Console.WriteLine(("Press any key to stop the server"));
            Console.ReadKey();
        }
    }
}