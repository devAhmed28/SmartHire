using FluentAssertions;
using SmartHire.Application.Common.Models;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Common
{
    public class ResultTests : TestBase
    {
        #region Success

        [Fact]
        public void Success_ShouldReturnIsSuccessTrue()
        {
            // Act
            var result = Result.Success();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.Error.Should().BeNull();
        }

        [Fact]
        public void Success_WithValue_ShouldReturnValue()
        {
            // Arrange
            var expectedValue = "Test Value";

            // Act
            var result = Result.Success(expectedValue);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(expectedValue);
            result.Error.Should().BeNull();
        }

        #endregion

        #region Failure

        [Fact]
        public void Failure_ShouldReturnIsSuccessFalse()
        {
            // Arrange
            var error = Error.Validation("Test error");

            // Act
            var result = Result.Failure(error);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(error);
        }

        [Fact]
        public void Failure_WithError_ShouldReturnError()
        {
            // Arrange
            var error = Error.NotFound("User");

            // Act
            var result = Result.Failure<string>(error);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
            result.Value.Should().BeNull();
        }

        #endregion

        #region Implicit Conversion

        [Fact]
        public void ImplicitConversion_ErrorToResult_ShouldReturnFailure()
        {
            // Arrange
            var error = Error.Validation("Invalid input");

            // Act
            Result result = error;

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(error);
        }

        [Fact]
        public void ImplicitConversion_ValueToResult_ShouldReturnSuccess()
        {
            // Arrange
            var value = "Test";

            // Act
            Result<string> result = value;

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(value);
            result.Error.Should().BeNull();
        }

        #endregion
    }
}
