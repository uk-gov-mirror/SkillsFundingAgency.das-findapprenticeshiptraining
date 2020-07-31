using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Validation;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.FAT.Domain.Validation.ValidationResult;
using SFA.DAS.FAT.Domain.Courses;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Queries.GetCourseProviderDetails
{
    public class WhenGettingCourseProviderDetails
    {
        [Test, MoqAutoData]
        public void Then_Throws_ValidationException_When_Request_Fails_Validation(
            GetProviderQuery request,
            string propertyName,
            [Frozen] Mock<IValidator<GetProviderQuery>> mockValidator,
            [Frozen] ValidationResult validationResult,
            [Frozen] Mock<ICourseService> mockService,
            GetProviderQueryHandler handler)
        {

            // Arrange
            validationResult.AddError(propertyName);
            mockValidator
                .Setup(validator => validator.ValidateAsync(It.IsAny<GetProviderQuery>()))
                .ReturnsAsync(validationResult);

            // Act
            var act = new Func<Task>(async () => await handler.Handle(request, CancellationToken.None));

            // Assert
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Query_Is_Valid_The_Service_Is_Called_And_The_Data_Returned(
            GetProviderQuery request,
            TrainingCourseProviderDetails courseProviderResponse,
            [Frozen] Mock<IValidator<GetProviderQuery>> mockValidator,
            [Frozen] ValidationResult validationResult,
            [Frozen] Mock<ICourseService> mockService,
            GetProviderQueryHandler handler)
        {
            //Arrange
            validationResult.ValidationDictionary.Clear();
            mockValidator.Setup(x => x.ValidateAsync(request)).ReturnsAsync(validationResult);
            mockService.Setup(x => x.GetCourseProviderDetails(request.ProviderId)).ReturnsAsync(courseProviderResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            mockService.Verify(x => x.GetCourseProviderDetails(request.ProviderId), Times.Once);
            Assert.IsNotNull(actual);
            actual.Provider.Should().BeEquivalentTo(courseProviderResponse.CourseProviderDetails);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Course_Provider_Returns_Null(
            GetProviderQuery request,
            Provider courseProviderResponse,
            [Frozen] Mock<IValidator<GetProviderQuery>> mockValidator,
            [Frozen] ValidationResult validationResult,
            [Frozen] Mock<ICourseService> mockService,
            GetProviderQueryHandler handler)
        {
            //Arrange
            validationResult.ValidationDictionary.Clear();
            mockValidator.Setup(x => x.ValidateAsync(request)).ReturnsAsync(validationResult);
            mockService.Setup(x => x.GetCourseProviderDetails(request.ProviderId)).ReturnsAsync((TrainingCourseProviderDetails)null);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);

            //Assert
            mockService.Verify(x => x.GetCourseProviderDetails(request.ProviderId), Times.Once);
            Assert.IsNull(actual.Provider);
        }
    }
}
