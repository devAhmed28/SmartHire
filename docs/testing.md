# SmartHire Testing

This document describes the testing approach used in the SmartHire backend.

The project contains tests at different levels to verify domain logic, application behavior, infrastructure components, and API endpoints.

---

## Testing Overview

SmartHire uses the following testing approaches:

- Unit Testing
- Integration Testing
- API Testing

The test project is located under:

```text
tests/
└── SmartHire.Tests/
```

The current test suite contains:

| Metric  | Result |
| ------- | -----: |
| Tests   |     86 |
| Passed  |     86 |
| Failed  |      0 |
| Skipped |      0 |

---

## Testing Technologies

The project uses:

- xUnit
- Moq
- FluentAssertions
- AutoFixture
- WebApplicationFactory

### xUnit

xUnit is used as the main testing framework.

### Moq

Moq is used to create mock dependencies when testing application components in isolation.

### FluentAssertions

FluentAssertions provides readable assertions for test results.

### AutoFixture

AutoFixture is used to create test data and reduce repetitive setup code.

### WebApplicationFactory

`WebApplicationFactory` is used for API-level testing through the ASP.NET Core application pipeline.

---

## Test Project Structure

The test project is organized into different areas:

```text
tests/
└── SmartHire.Tests/
    ├── ApiTests/
    ├── IntegrationTests/
    ├── UnitTests/
    ├── Customizations/
    ├── Fixtures/
    └── Helpers/
```

Each area has a different testing purpose.

---

## Unit Testing

Unit tests focus on testing individual pieces of application and domain logic in isolation.

The current unit tests cover areas such as:

### Domain Tests

Domain tests verify business rules and behavior of domain entities.

Covered entities include:

- `User`
- `Job`
- `Company`
- `CandidateProfile`

### Application Tests

Application tests cover commands and queries, including:

- Register
- Login
- Create Job
- Update Job
- Delete Job
- Apply for Job
- Get Job
- Get Job Applications
- Get My Applications

The tests verify expected behavior and business rules for the application handlers.

### Common Tests

Common application components are also tested, including:

- Result
- Error
- Validation-related behavior

---

## Unit Test Approach

Unit tests isolate the component being tested from its external dependencies.

Mocks are used when a handler or service depends on another component that is not part of the behavior being tested.

Test data can be generated using AutoFixture and assertions are written using FluentAssertions.

---

## Integration Testing

Integration tests verify that multiple parts of the application work together correctly.

The current integration tests cover areas such as:

### Repository Tests

Repository tests verify database-related repository behavior.

Current repository tests include:

- `JobRepositoryTests`
- `UserRepositoryTests`

These tests verify that repository operations work correctly with the application's persistence layer.

### Token Service Tests

The authentication token service is also tested through integration tests.

The tests verify token-related behavior and interaction with the authentication infrastructure.

### Database Testing

The test project contains database fixtures and helpers used to create and configure the test database environment.

The test infrastructure includes:

- Database fixtures
- Test database context factory
- Shared test helpers
- Test customizations

The purpose is to provide a controlled environment for tests that require database access.

---

## Integration Test Structure

The integration tests are organized under:

```text
tests/
└── SmartHire.Tests/
    └── IntegrationTests/
        ├── Repositories/
        └── Services/
```

Integration tests complement unit tests by verifying behavior across multiple application components.

## API Testing

API tests verify the behavior of the SmartHire HTTP API through the ASP.NET Core application pipeline.

The project uses `WebApplicationFactory` to create a test instance of the API application.

### Current API Tests

The current API test coverage includes authentication endpoints.

The authentication API tests verify scenarios such as:

- User registration
- Login
- Authentication responses
- Validation behavior
- Invalid authentication requests

API tests help verify that controllers, middleware, application services, and other required components work together through the HTTP layer.

---

## API Test Structure

API tests are located under:

```text
tests/
└── SmartHire.Tests/
    └── ApiTests/
        └── Controllers/
            └── AuthControllerTests.cs
```

Using `WebApplicationFactory` allows the tests to send HTTP requests to the application and verify the resulting responses.

---

## Test Organization

The test project is separated into different test levels:

```text
tests/SmartHire.Tests/
│
├── ApiTests/
│   └── Controllers/
│
├── IntegrationTests/
│   ├── Repositories/
│   └── Services/
│
├── UnitTests/
│   ├── Application/
│   ├── Common/
│   └── Domain/
│
├── Customizations/
├── Fixtures/
└── Helpers/
```

This structure keeps tests organized according to the part of the application they verify.

---

## Current Test Result

The current test suite contains:

| Result      |  Count |
| ----------- | -----: |
| Total Tests | **86** |
| Passed      | **86** |
| Failed      |  **0** |
| Skipped     |  **0** |

All currently implemented tests are passing.

---

## Testing Goals

The testing approach is intended to provide confidence across the main application layers:

```text
Domain Logic
     ↓
Application Logic
     ↓
Infrastructure
     ↓
HTTP API
```

Unit tests provide fast feedback for isolated logic, while integration and API tests verify interactions between multiple components.

---

## Running the Tests

From the solution root, run:

```bash
dotnet test
```

To run the tests for the specific test project:

```bash
dotnet test tests/SmartHire.Tests/SmartHire.Tests.csproj
```

The test command builds the test project, executes the available tests, and reports the final result.

---

## Summary

SmartHire uses multiple testing levels to verify the backend:

| Test Type         | Purpose                                                              |
| ----------------- | -------------------------------------------------------------------- |
| Unit Tests        | Verify individual domain and application components                  |
| Integration Tests | Verify interaction between application and infrastructure components |
| API Tests         | Verify HTTP endpoints through the ASP.NET Core pipeline              |

The combination of these tests helps verify the main business logic, persistence behavior, authentication services, and API functionality.

---
