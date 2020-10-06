using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Web.Models;
using DeliveryModeType = SFA.DAS.FAT.Web.Models.DeliveryModeType;

namespace SFA.DAS.FAT.Web.UnitTests.Models.CourseProvidersViewModelTests
{
    public class WhenGettingTotalMessage
    {
        [Test]
        [InlineAutoData(10, 5, false, "10 results")]
        [InlineAutoData(10, 5, true, "5 results")]
        [InlineAutoData(1, 5, false, "1 result")]
        [InlineAutoData(5, 1, true, "1 result")]
        [InlineAutoData(0, 5, false, "0 results")]
        [InlineAutoData(5, 0, true, "0 results")]
        public void Then_The_Total_Message_Is_Created_Correctly(
            int totalCount, 
            int filterTotal, 
            bool hasFilter, 
            string expectedMessage,
            GetCourseProvidersResult result)
        {
            //arrange
            var selectedDeliveryModes = new List<DeliveryModeType>();
            if (hasFilter)
            {
                selectedDeliveryModes.Add(DeliveryModeType.BlockRelease);
            }
            var request = new GetCourseProvidersRequest{DeliveryModes = selectedDeliveryModes};
            result.TotalFiltered = filterTotal;
            result.Total = totalCount;
            
            //act
            var viewModel = new CourseProvidersViewModel(request, result);

            //assert
            viewModel.TotalMessage.Should().Be(expectedMessage);
        }
    }
}
