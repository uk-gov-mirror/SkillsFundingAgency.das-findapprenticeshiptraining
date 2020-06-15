using System;
using System.Linq;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Validation;

namespace SFA.DAS.FAT.Domain.UnitTests.Validation
{
    public class WhenCreatingAValidationResult
    {
        [Test]
        public void Then_If_There_Are_No_Values_In_The_Dictionary_It_Is_Valid()
        {
            //Arrange Act
            var actual = new ValidationResult();
            
            //Assert
            Assert.IsTrue(actual.IsValid());
        }

        [Test]
        public void Then_If_There_Are_Values_In_The_Dictionary_It_Is_Not_Valid()
        {
            //Arrange
            var actual = new ValidationResult();
            
            //Act
            actual.AddError("test","test");
            
            //Assert
            Assert.IsFalse(actual.IsValid());
        }

        [Test]
        public void Then_The_Message_Is_Automatically_Created_From_The_PropertyName()
        {
            //Arrange
            var actual = new ValidationResult();
            
            //Act
            actual.AddError("test");
            
            //Assert
            Assert.AreEqual("test has not been supplied", actual.ValidationDictionary.First().Value); 
        }

        [Test]
        public void Then_The_Errors_Can_Be_Converted_To_Data_Annotations()
        {
            //Arrange
            var expectedErrorMessage = $"The following parameters have failed validation{Environment.NewLine}Test|Test has not been supplied{Environment.NewLine}Test2|Test2 has not been supplied";
            var actual = new ValidationResult();
            
            //Act
            actual.AddError("Test");
            actual.AddError("Test2");
            
            //Assert
            Assert.AreEqual(expectedErrorMessage, actual.DataAnnotationResult.ErrorMessage);
        }
    }
}