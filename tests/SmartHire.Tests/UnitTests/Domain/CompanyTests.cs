using AutoFixture;
using FluentAssertions;
using SmartHire.Domain.Entities;
using SmartHire.Domain.Enums;
using SmartHire.Tests.Helpers;

namespace SmartHire.Tests.UnitTests.Domain
{
    public class CompanyTests : TestBase
    {
        #region CreateCompany

        [Fact]
        public async Task CreateCompany_WithValidData_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(
                user.Id,
                "TechCorp Inc.",
                "We build amazing software",
                "Software Development",
                CompanySize.Medium
            );

            // Assert
            company.Id.Should().NotBe(Guid.Empty);
            company.UserId.Should().Be(user.Id);
            company.CompanyName.Should().Be("TechCorp Inc.");
            company.Description.Should().Be("We build amazing software");
            company.Industry.Should().Be("Software Development");
            company.CompanySize.Should().Be(CompanySize.Medium);
            company.IsVerified.Should().BeFalse();
            company.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        #endregion

        #region Verify

        [Fact]
        public async Task Verify_WhenCompanyIsNotVerified_ShouldSetIsVerifiedToTrue()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);
            company.IsVerified.Should().BeFalse();

            // Act
            company.Verify();

            // Assert
            company.IsVerified.Should().BeTrue();
            company.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region UpdateCompanyDetails

        [Fact]
        public async Task UpdateCompanyDetails_ShouldUpdateProperties()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "OldName", "OldDesc", "OldIndustry", CompanySize.Small);

            // Act
            company.UpdateCompanyDetails("NewName", "NewDesc", "NewIndustry", CompanySize.Large);

            // Assert
            company.CompanyName.Should().Be("NewName");
            company.Description.Should().Be("NewDesc");
            company.Industry.Should().Be("NewIndustry");
            company.CompanySize.Should().Be(CompanySize.Large);
            company.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region UpdateAddress

        [Fact]
        public async Task UpdateAddress_ShouldUpdateAddressProperties()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);

            // Act
            company.UpdateAddress("123 Main St", "New York", "USA");

            // Assert
            company.Address.Should().Be("123 Main St");
            company.City.Should().Be("New York");
            company.Country.Should().Be("USA");
            company.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region UpdateWebsite

        [Fact]
        public async Task UpdateWebsite_ShouldUpdateWebsiteUrl()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);

            // Act
            company.UpdateWebsite("https://www.techcorp.com");

            // Assert
            company.WebsiteUrl.Should().Be("https://www.techcorp.com");
            company.UpdatedAt.Should().NotBeNull();
        }

        #endregion

        #region UpdateLogo

        [Fact]
        public async Task UpdateLogo_ShouldUpdateLogoUrl()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var company = new Company(user.Id, "TechCorp", "Description", "Tech", CompanySize.Medium);

            // Act
            company.UpdateLogo("https://cloudinary.com/logo.jpg");

            // Assert
            company.LogoUrl.Should().Be("https://cloudinary.com/logo.jpg");
            company.UpdatedAt.Should().NotBeNull();
        }

        #endregion
    }

}
