# SmartHire Technologies

This document describes the main technologies and libraries used in the SmartHire backend.

---

## Backend

SmartHire is built using:

- C#
- ASP.NET Core Web API
- .NET 10

The API provides the HTTP endpoints used by candidates, companies, and administrators.

---

## Database

SmartHire uses **SQL Server** as its relational database.

### Technologies

- SQL Server
- Entity Framework Core
- Code First approach

Entity Framework Core is used to map the application's domain entities to database tables and to manage database operations.

Database changes are managed through **EF Core migrations**.

The Infrastructure layer contains the database context and persistence implementations.

---

## Authentication & Authorization

SmartHire uses JWT-based authentication and role-based authorization.

### Technologies

- JWT Bearer Authentication
- Refresh Tokens
- Role-Based Authorization

JWT access tokens are used to authenticate requests to protected API endpoints.

Refresh tokens are used to obtain new access tokens when the current access token expires.

Role-based authorization controls access to functionality based on the user's role:

- Candidate
- Company
- Admin

---

## Architecture & Libraries

The project uses several libraries and patterns to organize the application and keep responsibilities separated.

### Main Technologies

- Clean Architecture
- CQRS
- MediatR
- Repository Pattern
- Unit of Work
- Dependency Injection

### MediatR

MediatR is used to dispatch commands and queries between the API and Application layers.

### Entity Framework Core

Entity Framework Core is used for database access and persistence.

### Dependency Injection

ASP.NET Core's built-in Dependency Injection system is used to register and provide application and infrastructure services.

---

## Validation

SmartHire uses **FluentValidation** to validate application requests.

### Technologies

- FluentValidation
- MediatR Validation Pipeline Behavior

Validators define the validation rules for commands and other application requests.

The validation pipeline behavior runs these validators before the corresponding request handler executes.

This keeps validation logic separate from controllers and handlers.

---

## Validation

SmartHire uses **FluentValidation** to validate application requests.

### Technologies

- FluentValidation
- MediatR Validation Pipeline Behavior

Validators define the validation rules for commands and other application requests.

The validation pipeline behavior runs these validators before the corresponding request handler executes.

This keeps validation logic separate from controllers and handlers.

---

## Logging

SmartHire uses **Serilog** for application logging.

### Technologies

- Serilog
- ASP.NET Core logging integration

Serilog is configured during application startup and is used for application and error logging.

The API also includes request logging middleware for logging incoming HTTP requests.

Global exception handling middleware logs unexpected exceptions before returning an error response to the client.

---

## File Storage

SmartHire uses **Cloudinary** for storing uploaded files.

### Technologies

- Cloudinary

Cloudinary is used for files such as:

- Candidate CVs
- User profile images
- Company logos

The application contains services that handle file upload and deletion operations through the Cloudinary integration.

---

## API Documentation

SmartHire uses **Swagger / OpenAPI** to document and test the API endpoints.

### Technologies

- Swagger
- OpenAPI

Swagger provides an interactive interface where API endpoints can be viewed and tested during development.

The documented API is organized around the main application modules:

- Authentication
- Users
- Companies
- Candidates
- Jobs
- Applications
- Interviews
- Offers
- Notifications
- Reviews
- File Uploads
- Administration

The complete endpoint documentation is available in:

`docs/api.md`

---

## Testing

SmartHire uses several testing tools to verify the application at different levels.

### Technologies

- xUnit
- Moq
- FluentAssertions
- AutoFixture
- WebApplicationFactory

### Testing Levels

#### Unit Testing

Unit tests are used to test individual pieces of application and domain logic.

#### Integration Testing

Integration tests verify how multiple components work together, including repositories and services.

#### API Testing

API tests use `WebApplicationFactory` to test HTTP endpoints through the ASP.NET Core application pipeline.

The current test suite contains:

- 86 tests
- 86 passed
- 0 failed
- 0 skipped

Detailed testing information is available in:

`docs/testing.md`

---

## Technology Summary

The SmartHire backend currently uses the following main technologies:

| Area                | Technology                |
| ------------------- | ------------------------- |
| Language            | C#                        |
| Framework           | ASP.NET Core Web API      |
| Runtime             | .NET 10                   |
| Database            | SQL Server                |
| ORM                 | Entity Framework Core     |
| Authentication      | JWT Bearer Authentication |
| Authorization       | Role-Based Authorization  |
| Application Pattern | CQRS                      |
| Mediator            | MediatR                   |
| Validation          | FluentValidation          |
| Logging             | Serilog                   |
| File Storage        | Cloudinary                |
| API Documentation   | Swagger / OpenAPI         |
| Unit Testing        | xUnit                     |
| Mocking             | Moq                       |
| Assertions          | FluentAssertions          |
| Test Data           | AutoFixture               |
| API Testing         | WebApplicationFactory     |

---

## Future Technologies

The following technologies are planned for later versions of the project:

- Docker
- Redis
- SignalR
- GitHub Actions
- Cloud deployment

These are intentionally not part of the current implementation.
