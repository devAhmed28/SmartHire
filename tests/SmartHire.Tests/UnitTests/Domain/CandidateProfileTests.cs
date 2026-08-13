using AutoFixture;
using FluentAssertions;
using SmartHire.Domain.Entities;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Domain
{
    public class CandidateProfileTests : TestBase
    {
        #region CreateCandidateProfile

        [Fact]
        public async Task CreateCandidateProfile_WithValidData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Act
            var profile = new CandidateProfile(
                user.Id,
                "I am a passionate software developer",
                "Senior Software Engineer",
                "New York, NY",
                5,
                120000
            );

            // Assert
            profile.Id.Should().NotBe(Guid.Empty);
            profile.UserId.Should().Be(user.Id);
            profile.Bio.Should().Be("I am a passionate software developer");
            profile.CurrentPosition.Should().Be("Senior Software Engineer");
            profile.CurrentLocation.Should().Be("New York, NY");
            profile.YearsOfExperience.Should().Be(5);
            profile.ExpectedSalary.Should().Be(120000);
            profile.IsOpenToWork.Should().BeTrue();
            profile.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        #endregion

        #region UpdateProfile

        [Fact]
        public async Task UpdateProfile_ShouldUpdateProperties()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var profile = new CandidateProfile(
                user.Id,
                "Old Bio",
                "Old Position",
                "Old Location",
                2,
                50000
            );

            // Act
            profile.UpdateProfile(
                "New Bio",
                "New Position",
                "New Location",
                8,
                150000
            );

            // Assert
            profile.Bio.Should().Be("New Bio");
            profile.CurrentPosition.Should().Be("New Position");
            profile.CurrentLocation.Should().Be("New Location");
            profile.YearsOfExperience.Should().Be(8);
            profile.ExpectedSalary.Should().Be(150000);
            profile.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region UpdateSocialLinks

        [Fact]
        public async Task UpdateSocialLinks_ShouldUpdateLinks()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var profile = new CandidateProfile(
                user.Id,
                "Bio",
                "Position",
                "Location",
                3,
                60000
            );

            // Act
            profile.UpdateSocialLinks(
                "https://github.com/testuser",
                "https://linkedin.com/in/testuser",
                "https://portfolio.com/testuser"
            );

            // Assert
            profile.GitHubUrl.Should().Be("https://github.com/testuser");
            profile.LinkedInUrl.Should().Be("https://linkedin.com/in/testuser");
            profile.PortfolioUrl.Should().Be("https://portfolio.com/testuser");
            profile.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region SetOpenToWork

        [Fact]
        public async Task SetOpenToWork_ShouldUpdateStatus()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var profile = new CandidateProfile(
                user.Id,
                "Bio",
                "Position",
                "Location",
                3,
                60000
            );

            profile.IsOpenToWork.Should().BeTrue();

            // Act
            profile.SetOpenToWork(false);

            // Assert
            profile.IsOpenToWork.Should().BeFalse();
            profile.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region UpdateCV

        [Fact]
        public async Task UpdateCV_ShouldUpdateCVUrl()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var profile = new CandidateProfile(
                user.Id,
                "Bio",
                "Position",
                "Location",
                3,
                60000
            );

            var cvUrl = "https://cloudinary.com/cv.pdf";

            // Act
            profile.UpdateCV(cvUrl);

            // Assert
            profile.CVUrl.Should().Be(cvUrl);
            profile.UpdatedAt.Should().NotBeNull();
        }

        #endregion
    }
}
