using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetCourse;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Queries.GetCourse
{
    public class WhenValidatingAGetCourseRequest
    {
        private GetCourseRequestValidator _validator;

        [SetUp]
        public void Arrange()
        {
            _validator = new GetCourseRequestValidator();
        }
        
        [Test]
        public async Task Then_Is_Valid_Is_Returned_If_All_Values_Are_Valid()
        {
            //Act
            var actual = await _validator.ValidateAsync(new GetCourseRequest());
            
            //Assert
            Assert.IsFalse(actual.IsValid());
            Assert.IsTrue(actual.ValidationDictionary.ContainsKey(nameof(GetCourseRequest.CourseId)));
        }

        [Test]
        public async Task Then_Returns_Validation_Errors_If_Request_Is_Not_Valid()
        {
            //Act
            var actual = await _validator.ValidateAsync(new GetCourseRequest{CourseId = 10});
            
            //Assert
            Assert.IsTrue(actual.IsValid());
        }
    }
}