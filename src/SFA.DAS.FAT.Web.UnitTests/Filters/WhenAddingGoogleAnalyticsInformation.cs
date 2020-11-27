using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Filters;
using SFA.DAS.FAT.Web.Models;
using SFA.DAS.FAT.Web.UnitTests.Customisations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAT.Web.UnitTests.Filters
{
    public class WhenAddingGoogleAnalyticsInformation
    {
        [Test, MoqAutoData]
        public async Task Then_If_The_Context_Has_A_Location_Adds_The_Location_From_The_Context_To_The_ViewBag(
            string location,
            [ArrangeActionContext] ActionExecutingContext context,
            [Frozen] Mock<ActionExecutionDelegate> nextMethod,
            [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
            GoogleAnalyticsFilter filter)
        {
            //Arrange
            context.RouteData.Values.Add("location", location);
            cookieStorageService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(new LocationCookieItem());

            //Act
            await filter.OnActionExecutionAsync(context, nextMethod.Object);

            //Assert
            var actualController = context.Controller as Controller;
            Assert.IsNotNull(actualController);
            var viewBag = actualController.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBag);
            Assert.AreEqual(location, viewBag.Location);
        }

        [Test, MoqAutoData]
        public async Task
            Then_If_No_Location_In_Context_And_Location_In_Cookie_Adds_The_Location_From_The_Cookie_To_The_ViewBag(
                LocationCookieItem location,
                [ArrangeActionContext] ActionExecutingContext context,
                [Frozen] Mock<ActionExecutionDelegate> nextMethod,
                [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
                GoogleAnalyticsFilter filter)
        {
            //Arrange
            cookieStorageService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(location);

            //Act
            await filter.OnActionExecutionAsync(context, nextMethod.Object);

            //Assert
            var actualController = context.Controller as Controller;
            Assert.IsNotNull(actualController);
            var viewBag = actualController.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBag);
            Assert.AreEqual(location.Name, viewBag.Location);
        }
        [Test, MoqAutoData]
        public async Task
            Then_If_Location_In_Context_And_Location_In_Cookie_Adds_The_Location_From_The_Cookie_To_The_ViewBag(
                string location,
                LocationCookieItem cookieLocation,
                [ArrangeActionContext] ActionExecutingContext context,
                [Frozen] Mock<ActionExecutionDelegate> nextMethod,
                [Frozen] Mock<ICookieStorageService<LocationCookieItem>> cookieStorageService,
                GoogleAnalyticsFilter filter)
        {
            //Arrange
            context.RouteData.Values.Add("location", location);
            cookieStorageService.Setup(x => x.Get(Constants.LocationCookieName))
                .Returns(cookieLocation);

            //Act
            await filter.OnActionExecutionAsync(context, nextMethod.Object);

            //Assert
            var actualController = context.Controller as Controller;
            Assert.IsNotNull(actualController);
            var viewBag = actualController.ViewBag.GaData as GaData;
            Assert.IsNotNull(viewBag);
            Assert.AreEqual(location, viewBag.Location);
        }
    }
}
