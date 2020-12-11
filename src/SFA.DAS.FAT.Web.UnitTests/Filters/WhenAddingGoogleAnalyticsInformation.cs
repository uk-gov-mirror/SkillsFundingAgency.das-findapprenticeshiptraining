using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Controllers;
using SFA.DAS.FAT.Web.Filters;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Filters
{
    public class WhenAddingGoogleAnalyticsInformation
    {
        [Test, MoqAutoData]
        public async Task Then_If_The_Request_Has_A_Location_Adds_The_Location_To_The_ViewBag(
            string location,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
            [Greedy] CoursesController controller,
            GoogleAnalyticsFilter filter)
        {
            //Arrange
            var context =
                SetupContextAndCookieLocations(controller, location, null, cookieStorageService);

            //Act
            await filter.OnActionExecutionAsync(context, Mock.Of<ActionExecutionDelegate>());

            //Assert
            var viewBag = controller.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBag);
            Assert.AreEqual(location, viewBag.Location);
        }

        [Test, MoqAutoData]
        public async Task
            Then_If_No_Location_In_Context_And_Location_In_Cookie_Adds_The_Location_From_The_Cookie_To_The_ViewBag(
                LocationCookieItem location,
                [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
                [Greedy] CoursesController controller,
                GoogleAnalyticsFilter filter)
        {
            //Arrange
            var context = SetupContextAndCookieLocations(controller, null, location, cookieStorageService);

            //Act
            await filter.OnActionExecutionAsync(context, Mock.Of<ActionExecutionDelegate>());

            //Assert
            var viewBag = controller.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBag);
            Assert.AreEqual(location.Name, viewBag.Location);
        }
        [Test, MoqAutoData]
        public async Task
            Then_If_Location_In_Context_And_Location_In_Cookie_Adds_The_Location_From_The_Context_To_The_ViewBag(
                string location,
                LocationCookieItem cookieLocation,
                [Greedy] CoursesController controller,
                [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
                GoogleAnalyticsFilter filter)
        {
            //Arrange
            var context = SetupContextAndCookieLocations(controller, location, cookieLocation, cookieStorageService);

            //Act
            await filter.OnActionExecutionAsync(context, Mock.Of<ActionExecutionDelegate>());

            //Assert
            var viewBag = controller.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBag);
            Assert.AreEqual(location, viewBag.Location);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Location_In_Context_Or_Cookie_Location_Is_Not_Added_To_The_ViewBag(
            [Greedy] CoursesController controller,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
            GoogleAnalyticsFilter filter)
        {
            // Arrange
            var context = SetupContextAndCookieLocations(controller, null, null, cookieStorageService);

            //Act
            await filter.OnActionExecutionAsync(context, Mock.Of<ActionExecutionDelegate>());

            //Assert
            var viewBag = controller.ViewBag.GaData as GaData;
            viewBag.Should().BeEquivalentTo(new GaData());
        }

        private ActionExecutingContext SetupContextAndCookieLocations(CoursesController controller, string location,
            LocationCookieItem cookieLocation, Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService)
        {
            cookieStorageService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(cookieLocation);

            var httpContext = new DefaultHttpContext();
            if (!string.IsNullOrEmpty(location))
            {
                httpContext.Request.QueryString = new QueryString($"?location={location}");
            }
            
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var actionContext = new ActionContext(
                httpContext,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                new ModelStateDictionary()
            );

            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller
            );
        }
    }
}
