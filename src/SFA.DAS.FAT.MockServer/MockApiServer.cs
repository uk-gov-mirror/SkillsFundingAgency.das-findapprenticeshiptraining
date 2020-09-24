using System;
using System.Collections.Generic;
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
using WireMock.Types;

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
            
            server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s,"/trainingcourses/\\d+/providers/\\d+$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-provider-nolocation.json"));

            /*******************/
            server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s, "/trainingcourses/\\d+/providers/\\d+$"))
                .WithParam(MatchLocationParamCoventry)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-provider-details-notfound.json"));
            /*******************/


            server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s,"/trainingcourses/\\d+/providers/\\d+$"))
                .WithParam(MatchLocationParam)
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-provider.json"));

            server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s,"/trainingcourses/\\d+/providers$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-providers-nolocation.json"));
            
            server.Given(Request.Create()
                .WithPath(s => Regex.IsMatch(s,"/trainingcourses/\\d+/providers$"))
                .WithParam(MatchLocationParam)
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
                    
            server.Given(Request.Create().WithPath(IsLocation).UsingGet()).RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("locations.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s, "/trainingcourses/333$"))
                .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyFromFile("course-regulated.json"));

            server.Given(Request.Create().WithPath(s => Regex.IsMatch(s,"/trainingcourses/(?!(?:101|24|333)$)\\d+$"))
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

        private static bool MatchLocationParam(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("location") && arg["location"].Count !=0 && arg["location"].ToString().Length > 0;
        }

        private static bool MatchLocationParamCoventry(IDictionary<string, WireMockList<string>> arg)
        {
            return arg.ContainsKey("location") && arg["location"].Count != 0 && arg["location"].ToString().Equals("Coventry", StringComparison.CurrentCultureIgnoreCase);        
        }

        private static bool IsLocation(string arg)
        {
            return Regex.IsMatch(arg, @"/locations");
        }
    }
}
