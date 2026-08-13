using FluentAssertions;
using SmartHire.Application.Common.Models;
using SmartHire.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartHire.Tests.UnitTests.Common
{
    public class ErrorTests : TestBase
    {
        #region Create Error

        [Fact]
        public void CreateError_ShouldSetCodeAndMessage()
        {
            // Arrange
            var code = "Test.Code";
            var message = "Test message";

            // Act
            var error = new Error(code, message);

            // Assert
            error.Code.Should().Be(code);
            error.Message.Should().Be(message);
        }

        #endregion

        #region Static Error Methods

        [Fact]
        public void NotFound_ShouldReturnNotFoundError()
        {
            // Act
            var error = Error.NotFound("User");

            // Assert
            error.Code.Should().Be("NotFound");
            error.Message.Should().Be("User was not found");
        }

        [Fact]
        public void Conflict_ShouldReturnConflictError()
        {
            // Act
            var error = Error.Conflict("Email already exists");

            // Assert
            error.Code.Should().Be("Conflict");
            error.Message.Should().Be("Email already exists");
        }

        [Fact]
        public void Validation_ShouldReturnValidationError()
        {
            // Act
            var error = Error.Validation("Invalid input");

            // Assert
            error.Code.Should().Be("Validation.Error");
            error.Message.Should().Be("Invalid input");
        }

        [Fact]
        public void Unauthorized_ShouldReturnUnauthorizedError()
        {
            // Act
            var error = Error.Unauthorized();

            // Assert
            error.Code.Should().Be("Unauthorized");
            error.Message.Should().Be("You are not authorized");
        }

        [Fact]
        public void Forbidden_ShouldReturnForbiddenError()
        {
            // Act
            var error = Error.Forbidden();

            // Assert
            error.Code.Should().Be("Forbidden");
            error.Message.Should().Be("Access denied");
        }

        [Fact]
        public void Internal_ShouldReturnInternalError()
        {
            // Act
            var error = Error.Internal();

            // Assert
            error.Code.Should().Be("Internal");
            error.Message.Should().Be("An unexpected error occurred");
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var error = new Error("Test.Code", "Test message");

            // Act
            var result = error.ToString();

            // Assert
            result.Should().Be("Test.Code: Test message");
        }

        #endregion
    }
}
