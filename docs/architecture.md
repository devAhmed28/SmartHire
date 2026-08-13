# SmartHire Architecture

This document describes the architecture and main design patterns used in the SmartHire backend.

---

## Architecture Overview

SmartHire follows **Clean Architecture** to separate business rules, application logic, infrastructure concerns, and the API layer.

The main layers are:

- **Domain**
- **Application**
- **Infrastructure**
- **API**

The project also uses **CQRS with MediatR** to separate commands that change state from queries that retrieve data.

---

## Domain Layer

The Domain layer contains the core business concepts of SmartHire.

It is the innermost layer and should not depend on the Application, Infrastructure, or API layers.

### Main Responsibilities

- Define domain entities
- Define enums
- Represent core business rules
- Maintain domain state and behavior

### Examples

The Domain layer contains entities such as:

- User
- Company
- CandidateProfile
- Job
- JobApplication
- Interview
- Offer
- Review
- Notification
- SavedJob
- RefreshToken
- Skill

The Domain layer focuses on **business concepts rather than technical details** such as databases, HTTP requests, or external services.

---

## Application Layer

The Application layer contains the application's use cases and coordinates the business operations of the system.

It depends on the Domain layer but does not directly depend on the API or Infrastructure implementations.

### Main Responsibilities

- Define commands and queries
- Implement command and query handlers
- Define DTOs
- Define application interfaces
- Validate incoming requests
- Coordinate business operations
- Return standardized application results

### CQRS

SmartHire uses **CQRS (Command Query Responsibility Segregation)**.

Commands are used for operations that change application state, such as:

- Registering a user
- Creating a job
- Applying for a job
- Updating an application

Queries are used for retrieving data, such as:

- Getting a job
- Getting user applications
- Getting job applications

MediatR is used to dispatch commands and queries to their handlers.

### Validation

Request validation is implemented using **FluentValidation**.

A MediatR validation behavior runs validators before the corresponding request handler executes.

This keeps validation separate from the controllers and application handlers.

---

## Infrastructure Layer

The Infrastructure layer contains the technical implementations required by the application.

It implements interfaces defined by the Application layer and handles external concerns such as database access and external services.

### Main Responsibilities

- Entity Framework Core database access
- Repository implementations
- Unit of Work implementation
- Database configuration
- Authentication and token services
- File storage services
- External service integrations

### Persistence

SmartHire uses **Entity Framework Core with SQL Server**.

The Infrastructure layer contains the database context and repository implementations used to access and persist application data.

The Application layer depends on abstractions such as repositories and `IUnitOfWork`, while Infrastructure provides their concrete implementations.

### External Services

Infrastructure also contains integrations with external services such as **Cloudinary** for file storage.

This keeps external service details outside the Domain and Application business logic.

---

## API Layer

The API layer is the entry point of the application. It exposes the application's functionality through HTTP endpoints.

### Main Responsibilities

- Define API controllers
- Handle HTTP requests and responses
- Configure authentication and authorization
- Configure middleware
- Configure dependency injection
- Configure Swagger / OpenAPI
- Configure the application pipeline

### Controllers

Controllers receive HTTP requests and translate them into application requests.

The controllers do not contain the main business logic. Instead, they send commands and queries to the Application layer through MediatR.

A simplified flow is:

HTTP Request
↓
Controller
↓
MediatR
↓
Command / Query
↓
Handler
↓
Repository / Unit of Work
↓
Database

### Middleware

The API layer also contains middleware for cross-cutting concerns such as:

- Global exception handling
- Request logging

This keeps these concerns outside individual controllers.

---

## Dependency Flow

The project follows a dependency direction that keeps the core business logic independent from technical details.

````text
SmartHire.API
      ↓
SmartHire.Application
      ↓
SmartHire.Domain

SmartHire.Infrastructure
      ↓
SmartHire.Application
      ↓
SmartHire.Domain

## CQRS and MediatR

SmartHire uses **CQRS (Command Query Responsibility Segregation)** to separate operations that change data from operations that retrieve data.

### Commands

Commands represent operations that modify the application state.

Examples include:

- Registering a user
- Creating a job
- Updating a job
- Applying for a job
- Updating an application status
- Deleting a job

Each command has a corresponding handler responsible for executing the use case.

### Queries

Queries are used to retrieve information without changing application state.

Examples include:

- Getting a job
- Getting job applications
- Getting user applications
- Getting job details

Each query has a corresponding handler responsible for retrieving and mapping the required data.

### MediatR

**MediatR** is used as the communication mechanism between the API layer and the Application layer.

Instead of controllers directly calling application handlers, they send requests through MediatR.

The general flow is:

```text
Controller
    ↓
MediatR
    ↓
Command / Query
    ↓
Handler
    ↓
Repository / Unit of Work

## Repository Pattern and Unit of Work

SmartHire uses the **Repository Pattern** and **Unit of Work Pattern** to separate application logic from database access.

### Repository Pattern

Repositories provide an abstraction for accessing and working with entities.

The Application layer works with repository interfaces instead of directly depending on Entity Framework Core implementations.

Examples include repositories for entities such as:

- Users
- Companies
- Jobs
- Applications
- Skills
- Saved Jobs

This keeps database access logic separated from the application use cases.

### Unit of Work

The `IUnitOfWork` abstraction groups database operations together and provides a single point for saving changes.

A typical application operation can:

1. Retrieve required data through repositories
2. Create or modify entities
3. Add related entities
4. Save the changes through the Unit of Work

For example:

```text
Command Handler
      ↓
IUnitOfWork
      ↓
Repositories
      ↓
Entity Framework Core
      ↓
SQL Server

## Validation Pipeline Behavior

SmartHire uses **FluentValidation** together with a MediatR pipeline behavior to validate requests before they reach their handlers.

### How It Works

When a command or query is sent through MediatR:

```text
Controller
    ↓
MediatR
    ↓
Validation Behavior
    ↓
Validator
    ↓
Handler

## Result Pattern and Error Handling

SmartHire uses a **Result Pattern** to represent successful and failed application operations without relying on exceptions for expected business outcomes.

### Result Pattern

Application operations return a `Result` that can represent:

- A successful operation
- A failed operation
- An error with a specific error type and message

This provides a consistent way for handlers to communicate the outcome of an operation to the API layer.

For example, an operation can return errors such as:

- Validation errors
- Not found errors
- Conflict errors
- Unauthorized errors
- Forbidden errors
- Internal errors

### Global Exception Handling

Unexpected exceptions are handled by global exception-handling middleware in the API layer.

The middleware:

1. Catches unhandled exceptions
2. Logs the exception using Serilog
3. Returns a consistent JSON error response
4. Prevents implementation details from being exposed to API clients

This keeps exception handling out of individual controllers and provides a consistent API response for unexpected failures.

### Why Use Both?

The Result Pattern and exception handling serve different purposes.

```text
Expected application outcome
        ↓
     Result
        ↓
     API response

## Authentication and Authorization

SmartHire uses **JWT Bearer Authentication** to authenticate API requests.

### Authentication

The authentication flow includes:

- User registration
- Login
- JWT access tokens
- Refresh tokens
- Logout

After a successful login, the API issues an access token that the client can use to access protected endpoints.

Refresh tokens are used to obtain a new access token when the current access token expires.

### Authorization

The API uses **role-based authorization** to control access to protected operations.

The main roles are:

- Candidate
- Company
- Admin

Controllers and endpoints can require a specific role before allowing an operation.

For example:

```text
Candidate
   ↓
Candidate endpoints

Company
   ↓
Company endpoints

Admin
   ↓
Administrative endpoints

## Dependency Injection and Configuration

SmartHire uses **Dependency Injection (DI)** to provide application services and infrastructure implementations where they are needed.

### Dependency Injection

Services are registered during application startup and injected into controllers, handlers, and other services through their constructors.

For example:

```text
Controller / Handler
        ↓
    Interface
        ↓
Concrete Implementation

## Logging and Cross-Cutting Concerns

SmartHire handles concerns that are shared across multiple parts of the application outside of individual business operations.

### Serilog

The application uses **Serilog** for structured logging.

Logging is configured at application startup and is used to record application events and errors.

### Request Logging

The API includes request logging middleware that can record information about incoming HTTP requests and their processing.

This keeps request logging separate from individual controllers.

### Exception Logging

Unexpected exceptions are logged by the global exception-handling middleware.

This provides a central place to record failures instead of adding exception logging to every controller.

### Middleware Pipeline

The API uses middleware for cross-cutting concerns such as:

- Exception handling
- Request logging
- CORS
- Authentication
- Authorization

A simplified pipeline is:

```text
HTTP Request
     ↓
Exception Handling
     ↓
Request Logging
     ↓
CORS
     ↓
Authentication
     ↓
Authorization
     ↓
Controller
     ↓
HTTP Response

## Testing Architecture

SmartHire includes tests at multiple levels to verify different parts of the application.

### Unit Tests

Unit tests focus on individual pieces of application and domain logic without requiring the full application to run.

The tests cover areas such as:

- Domain entities
- Application command handlers
- Application query handlers
- Result and Error models

Mocks and test customizations are used where dependencies need to be isolated.

### Integration Tests

Integration tests verify that multiple components work together correctly.

The current integration tests cover areas such as:

- Repository behavior
- Database-related functionality
- Token service behavior

### API Tests

API tests verify HTTP endpoints using ASP.NET Core's `WebApplicationFactory`.

The current API tests include authentication endpoint testing.

### Testing Structure

The test project is organized separately from the application projects:

```text
tests/
└── SmartHire.Tests/
    ├── ApiTests/
    ├── IntegrationTests/
    ├── UnitTests/
    ├── Customizations/
    ├── Fixtures/
    └── Helpers/
````
