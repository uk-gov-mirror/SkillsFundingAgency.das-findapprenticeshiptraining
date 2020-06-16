using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Domain.Courses;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Validation;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.FAT.Domain.Validation.ValidationResult;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Queries.GetCourse
{
    public class WhenGettingCourse
    {
        [Test, MoqAutoData]
        public void And_Fails_Validation_Then_Throws_ValidationException(
            GetCourseRequest request,
            string propertyName,
            [Frozen] Mock<IValidator<GetCourseRequest>> mockValidator,
            [Frozen] ValidationResult validationResult,
            [Frozen] Mock<ICourseService> mockService,
            GetCourseRequestHandler handler)
        {
            //Arrange
            validationResult.AddError(propertyName);
            mockValidator
                .Setup(validator => validator.ValidateAsync(It.IsAny<GetCourseRequest>()))
                .ReturnsAsync(validationResult);

            //Act
            var act = new Func<Task>(async () => await handler.Handle(request, CancellationToken.None));

            //Assert
            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Query_Is_Valid_The_Service_Is_Called_And_The_Data_Returned(
            GetCourseRequest request,
            Course courseResponse,
            [Frozen] Mock<IValidator<GetCourseRequest>> mockValidator,
            [Frozen] ValidationResult validationResult,
            [Frozen] Mock<ICourseService> mockService,
            GetCourseRequestHandler handler)
        {
            //Arrange
            validationResult.ValidationDictionary.Clear();
            mockValidator.Setup(x => x.ValidateAsync(request)).ReturnsAsync(validationResult);
            mockService.Setup(x => x.GetCourse(request.CourseId)).ReturnsAsync(courseResponse);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);
            
            //Assert
            mockService.Verify(x=>x.GetCourse(request.CourseId), Times.Once);
            Assert.IsNotNull(actual);
            actual.Course.Should().BeEquivalentTo(courseResponse);
        }
        
        [Test, MoqAutoData]
        public async Task And_No_Course_Then_Returns_Null(
            GetCourseRequest request,
            Course courseResponse,
            [Frozen] Mock<IValidator<GetCourseRequest>> mockValidator,
            [Frozen] ValidationResult validationResult,
            [Frozen] Mock<ICourseService> mockService,
            GetCourseRequestHandler handler)
        {
            //Arrange
            validationResult.ValidationDictionary.Clear();
            mockValidator.Setup(x => x.ValidateAsync(request)).ReturnsAsync(validationResult);
            mockService.Setup(x => x.GetCourse(request.CourseId)).ReturnsAsync((Course)null);

            //Act
            var actual = await handler.Handle(request, CancellationToken.None);
            
            //Assert
            mockService.Verify(x=>x.GetCourse(request.CourseId), Times.Once);
            Assert.IsNull(actual.Course);
        }
    }
}