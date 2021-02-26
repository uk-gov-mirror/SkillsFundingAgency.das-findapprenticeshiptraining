using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourses;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Controllers.HomeControllerTests
{
    public class WhenGettingTheSitemap
    {
        [Test, MoqAutoData]
        public async Task Then_The_Routes_Are_Added(
            string serviceStartUrl,
            string shortlistUrl,
            string privacyUrl,
            string accessibilityUrl,
            string cookiesUrl,
            string cookiesDetailsUrl,
            string coursesUrl,
            GetCoursesResult result,
            [Frozen] Mock<IMediator> mediator)
        {
            var httpContext = new DefaultHttpContext();
            var urlHelper = new Mock<IUrlHelper>();
            
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.ServiceStart))))
                .Returns(serviceStartUrl);
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.Courses))))
                .Returns(coursesUrl);
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.ShortList))))
                .Returns(shortlistUrl);
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.Privacy))))
                .Returns(privacyUrl);
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.AccessibilityStatement))))
                .Returns(accessibilityUrl);
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.Cookies))))
                .Returns(cookiesUrl);
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.CookiesDetails))))
                .Returns(cookiesDetailsUrl);
            mediator.Setup(x => x.Send(It.Is<GetCoursesQuery>(c => 
                    c.Keyword.Equals("")
                    && c.Levels == null
                    && c.RouteIds == null
                    && c.ShortlistUserId == null
                    && c.OrderBy.Equals(OrderBy.Name)
                ), CancellationToken.None))
                .ReturnsAsync(result);
            var controller = new HomeController(mediator.Object) {
                Url = urlHelper.Object, 
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            
            //Act
            var actual = await controller.SitemapXml() as ContentResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var document = new XmlDocument();
            document.LoadXml(actual.Content);
            
            var newNode  = XDocument.Parse(actual.Content).Root;
            Assert.IsNotNull(newNode);
            var nodes = newNode.Descendants(newNode.GetDefaultNamespace()+"loc").ToList();
            nodes.Any(c => c.Value.Equals(serviceStartUrl)).Should().BeTrue();
            nodes.Any(c => c.Value.Equals(coursesUrl)).Should().BeTrue();
            nodes.Any(c => c.Value.Equals(shortlistUrl)).Should().BeTrue();
            nodes.Any(c => c.Value.Equals(privacyUrl)).Should().BeTrue();
            nodes.Any(c => c.Value.Equals(accessibilityUrl)).Should().BeTrue();
            nodes.Any(c => c.Value.Equals(cookiesUrl)).Should().BeTrue();
            nodes.Any(c => c.Value.Equals(cookiesDetailsUrl)).Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Courses_Query_Is_Called_And_Nodes_Built_For_Each_Course_And_Provider_List(
            string courseUrl,
            string courseProvidersUrl,
            GetCoursesResult result,
            [Frozen] Mock<IMediator> mediator)
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var urlHelper = new Mock<IUrlHelper>();
            
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.CourseDetails))))
                .Returns(courseUrl);
            urlHelper
                .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName.Equals(RouteNames.CourseProviders))))
                .Returns(courseProvidersUrl);

            var controller = new HomeController(mediator.Object) {
                Url = urlHelper.Object, 
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
            mediator.Setup(x => x.Send(It.Is<GetCoursesQuery>(c => 
                    c.Keyword.Equals("")
                    && c.Levels == null
                    && c.RouteIds == null
                    && c.ShortlistUserId == null
                    && c.OrderBy.Equals(OrderBy.Name)
                    ), CancellationToken.None))
                .ReturnsAsync(result);
            
            //Act
            var actual = await controller.SitemapXml() as ContentResult;
            
            //Assert
            Assert.IsNotNull(actual);
            var document = new XmlDocument();
            document.LoadXml(actual.Content);
            
            var newNode  = XDocument.Parse(actual.Content).Root;
            Assert.IsNotNull(newNode);
            var nodes = newNode.Descendants(newNode.GetDefaultNamespace()+"loc").ToList();

            nodes.Count(c => c.Value.Equals(courseUrl)).Should().Be(result.Courses.Count);
            nodes.Count(c => c.Value.Equals(courseProvidersUrl)).Should().Be(result.Courses.Count);
            
        }
    }
}