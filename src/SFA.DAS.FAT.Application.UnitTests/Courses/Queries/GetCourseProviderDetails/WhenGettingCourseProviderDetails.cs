using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Domain.Validation;
using SFA.DAS.Testing.AutoFixture;

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
                .ReturnsAsync(validationResult)

            // Act
            var act = new Func<Task>(async () => await handler.Handle(request, CancellationToken.None));

            // Assert
            act.Should().Throw<ValidateException>().WithMessage($"*{propertyName}");

        }
    }
}
