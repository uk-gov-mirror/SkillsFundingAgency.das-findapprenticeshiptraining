using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WireMock.Logging;
using WireMock.Matchers;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.FAT.MockServer
{
    public static class MockApiServer
    {
        public static IWireMockServer Start()
        {
            var settings = new WireMockServerSettings
            {
                Port = 5003,
                Logger = new WireMockConsoleLogger()
            };
            
            var server = StandAloneApp.Start(settings);
            
            /*server.Given(Request.Create().WithPath("/trainingcourses")
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("courses.json"));

            server.Given(Request.Create()
                .WithPath(IsCourse)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course.json"));

            server.Given(Request.Create()
                .WithPath(IsExpiredCourse)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-expired.json"));

            server.Given(Request.Create().WithPath(IsListOfProviders)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-providers.json"));
            
            server.Given(Request.Create().WithPath(IsCourseProvider)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-provider.json"));*/

            // gnu way

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/trainingcourses/\\d+/providers/\\d+$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-provider.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/trainingcourses/\\d+/providers$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-providers.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/trainingcourses/101$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-expired.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/trainingcourses/24$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-lastdatestarts.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/trainingcourses/\\d+$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/trainingcourses$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("courses.json"));

            return server;
        }

        /*private static bool IsCourse(string arg)
        {
            return !arg.Contains("providers", StringComparison.CurrentCultureIgnoreCase) 
                   && !Regex.IsMatch(arg, @"/trainingcourses/101") 
                   && Regex.IsMatch(arg, @"/trainingcourses/[0-9]*");
        }*/

        /*private static bool IsExpiredCourse(string arg)
        {
            return !arg.Contains("providers", StringComparison.CurrentCultureIgnoreCase)
                   && Regex.IsMatch(arg, @"/trainingcourses/101");
        }*/

        /*private static bool IsListOfProviders(string arg)
        {
            var partCount = arg.Split("/");

            if (partCount.Length == 4 
                && partCount[1].Equals("trainingcourses", StringComparison.CurrentCultureIgnoreCase) 
                && partCount[3].Equals("providers", StringComparison.CurrentCultureIgnoreCase))
            {
                return Regex.IsMatch(arg, @"/trainingcourses/[0-9]#1#providers");
            }
            return false;
            
        }*/
        
        /*private static bool IsCourseProvider(string arg)
        {
            return Regex.IsMatch(arg, @"/trainingcourses/[0-9]#1#providers/[0-9]*");
        }*/

    }
}
