# SmartHire Development Roadmap

This document describes the current development status of SmartHire and the features planned for future versions.

---

## Current Status

The core recruitment backend has been implemented.

The current project includes:

- Authentication
- User management
- Company management
- Job management
- Candidate features
- Job applications
- Interviews
- Offers
- Notifications
- Reviews
- File uploads
- Administration
- Unit tests
- Integration tests
- API tests
- Swagger / OpenAPI documentation

The current test suite contains:

- 86 tests
- 86 passed
- 0 failed
- 0 skipped

---

## Completed Work

The main backend development has been completed and the implemented features have been tested.

### Backend

- Core recruitment workflow
- Clean Architecture
- CQRS with MediatR
- Repository Pattern
- Unit of Work
- Dependency Injection
- FluentValidation
- Global Exception Handling
- Serilog Logging
- JWT Authentication
- Role-Based Authorization

### Recruitment Features

- Candidate registration and management
- Company registration and management
- Job creation and management
- Job applications
- Interviews
- Offers
- Notifications
- Reviews
- Saved jobs
- File uploads
- Admin management

### Testing

- Domain unit tests
- Application unit tests
- Repository integration tests
- Service integration tests
- API tests

---

## Future Improvements

The following improvements are planned for future versions of the project.

### Docker Containerization

Containerize the application and its dependencies using Docker.

Possible goals:

- Dockerfile for the API
- Containerized database development environment
- Docker Compose configuration

### Redis Caching

Introduce Redis for caching frequently accessed data.

Possible use cases:

- Frequently requested jobs
- Job search results
- Frequently accessed platform data

### SignalR

Add real-time communication capabilities using SignalR.

Possible use cases:

- Real-time notifications
- Interview updates
- Application status updates

### GitHub Actions CI/CD

Introduce automated CI/CD using GitHub Actions.

Possible workflow:

```text
Push Code
   ↓
Build
   ↓
Run Tests
   ↓
Validate
   ↓
Deploy
```

### Cloud Deployment

Deploy the SmartHire backend to a cloud environment.

Possible goals:

- Cloud-hosted API
- Cloud database
- Secure environment configuration
- Automated deployment

### Additional Test Coverage

Expand the existing test suite with additional unit, integration, and API tests as new features are introduced.

---

## Roadmap

The planned development path is:

```text
Core Backend
     ↓
Testing
     ↓
Documentation
     ↓
Docker
     ↓
Redis
     ↓
SignalR
     ↓
CI/CD
     ↓
Cloud Deployment
```

The future improvements are intentionally planned for later versions and are not part of the current implementation.

---

## Project Direction

The goal of the roadmap is to gradually move SmartHire from a completed backend portfolio project toward a more production-oriented system.

Future work will focus on:

- Deployment
- Scalability
- Performance
- Automation
- Real-time functionality
- Infrastructure improvements
- Additional test coverage
