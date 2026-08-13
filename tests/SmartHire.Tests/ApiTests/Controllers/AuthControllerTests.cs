using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SmartHire.Application.DTOs.Auth;
using SmartHire.Domain.Enums;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SmartHire.Tests.ApiTests.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private static string CreateUniqueEmail(string prefix)
        {
            return $"{prefix}_{Guid.NewGuid():N}@example.com";
        }

        private static string CreateUniquePhone()
        {
            return $"+1555{Random.Shared.Next(1000000, 9999999)}";
        }

        #region Register

        [Fact]
        public async Task Register_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var email = CreateUniqueEmail("testuser");
            var phone = CreateUniquePhone();

            var request = new RegisterRequest
            {
                AccountType = AccountType.Candidate,
                Email = email,
                Password = "Password123!",
                PhoneNumber = phone,
                Candidate = new CandidateInfo
                {
                    FirstName = "Test",
                    LastName = "User"
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<AuthResponse>();

            result.Should().NotBeNull();
            result.Email.Should().Be(email);
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ShouldReturnConflict()
        {
            // Arrange
            var email = CreateUniqueEmail("duplicate");
            var phone = CreateUniquePhone();

            var request = new RegisterRequest
            {
                AccountType = AccountType.Candidate,
                Email = email,
                Password = "Password123!",
                PhoneNumber = phone,
                Candidate = new CandidateInfo
                {
                    FirstName = "Test",
                    LastName = "User"
                }
            };

            // First registration
            var firstResponse = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Act - same email AND same phone
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Register_WithInvalidEmail_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                AccountType = AccountType.Candidate,
                Email = "invalid-email",
                Password = "Password123!",
                PhoneNumber = CreateUniquePhone(),
                Candidate = new CandidateInfo
                {
                    FirstName = "Test",
                    LastName = "User"
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_WithoutCandidateDetails_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new RegisterRequest
            {
                AccountType = AccountType.Candidate,
                Email = CreateUniqueEmail("nocandidate"),
                Password = "Password123!",
                PhoneNumber = CreateUniquePhone(),
                Candidate = null
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Login

        [Fact]
        public async Task Login_WithInvalidEmail_ShouldReturnNotFound()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = CreateUniqueEmail("nonexistent"),
                Password = "WrongPassword!"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                loginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ShouldReturnBadRequest()
        {
            // Arrange
            var email = CreateUniqueEmail("loginuser");
            var phone = CreateUniquePhone();

            var registerRequest = new RegisterRequest
            {
                AccountType = AccountType.Candidate,
                Email = email,
                Password = "Password123!",
                PhoneNumber = phone,
                Candidate = new CandidateInfo
                {
                    FirstName = "Login",
                    LastName = "User"
                }
            };

            var registerResponse = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                registerRequest);

            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = "WrongPassword!"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/login",
                loginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Refresh Token

        [Fact]
        public async Task Refresh_WithValidToken_ShouldReturnOk()
        {
            // Arrange
            var email = CreateUniqueEmail("refreshuser");
            var phone = CreateUniquePhone();

            var registerRequest = new RegisterRequest
            {
                AccountType = AccountType.Candidate,
                Email = email,
                Password = "Password123!",
                PhoneNumber = phone,
                Candidate = new CandidateInfo
                {
                    FirstName = "Refresh",
                    LastName = "User"
                }
            };

            var registerResponse = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                registerRequest);

            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var registerResult = await registerResponse.Content
                .ReadFromJsonAsync<AuthResponse>();

            registerResult.Should().NotBeNull();
            registerResult.RefreshToken.Should().NotBeNullOrEmpty();

            var refreshRequest = new RefreshTokenRequest
            {
                RefreshToken = registerResult.RefreshToken
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/refresh",
                refreshRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content
                .ReadFromJsonAsync<AuthResponse>();

            result.Should().NotBeNull();
            result.AccessToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Refresh_WithInvalidToken_ShouldReturnNotFound()
        {
            // Arrange
            var refreshRequest = new RefreshTokenRequest
            {
                RefreshToken = $"invalid-token-{Guid.NewGuid():N}"
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/api/Auth/refresh",
                refreshRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region Logout

        [Fact]
        public async Task Logout_WhenAuthenticated_ShouldReturnOk()
        {
            // Arrange
            var email = CreateUniqueEmail("logoutuser");
            var phone = CreateUniquePhone();

            var registerRequest = new RegisterRequest
            {
                AccountType = AccountType.Candidate,
                Email = email,
                Password = "Password123!",
                PhoneNumber = phone,
                Candidate = new CandidateInfo
                {
                    FirstName = "Logout",
                    LastName = "User"
                }
            };

            var registerResponse = await _client.PostAsJsonAsync(
                "/api/Auth/register",
                registerRequest);

            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var registerResult = await registerResponse.Content
                .ReadFromJsonAsync<AuthResponse>();

            registerResult.Should().NotBeNull();
            registerResult.AccessToken.Should().NotBeNullOrEmpty();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    registerResult.AccessToken);

            // Act
            var response = await _client.PostAsync(
                "/api/Auth/logout",
                null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Logout_WhenNotAuthenticated_ShouldReturnUnauthorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _client.PostAsync(
                "/api/Auth/logout",
                null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        #endregion
    }
}