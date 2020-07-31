using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.FAT.Application.Courses.Queries.GetProvider;

namespace SFA.DAS.FAT.Application.UnitTests.Courses.Queries.GetCourseProviderDetails
{
    public class WhenValidatingAGetCourseProviderDetailsQuery
    {
        private GetCourseProviderDetailsQueryValidator _validator;
        
        [SetUp]
        public void Arrange()
        {
            _validator = new GetCourseProviderDetailsQueryValidator();
        }

        [Test]
        public async Task Then_Returns_Validation_Errors_If_Request_Is_Not_Valid()
        {
            //Act
            var actual = await _validator.ValidateAsync(new GetProviderQuery());

            //Assert
            Assert.IsFalse(actual.IsValid());
            Assert.IsTrue(actual.ValidationDictionary.ContainsKey(nameof(GetProviderQuery.ProviderId)));
        }

        [Test]
        public async Task Then_Is_Valid_Is_Returned_If_All_Values_Are_Valid()
        {
            //Act
            var actual = await _validator.ValidateAsync(new GetProviderQuery { ProviderId = 10 });

            //Assert
            Assert.IsTrue(actual.IsValid());
        }
    }
}
