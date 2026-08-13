# SmartHire - Recruitment Platform API

SmartHire is a recruitment platform backend built with **C# and ASP.NET Core Web API** as a portfolio project.

The project connects **Companies, Candidates, and Admins** and covers the main recruitment workflow.

---

## Features

- Authentication
- User Management
- Company Management
- Job Management
- Candidate Features
- Applications
- Interviews
- Offers
- Notifications
- Reviews
- File Uploads
- Admin

📄 **[Full Features Documentation](docs/features.md)**

---

## Architecture

- Clean Architecture
- CQRS + MediatR
- Repository Pattern + Unit of Work
- Dependency Injection
- Result Pattern
- FluentValidation
- Validation Pipeline Behavior
- JWT Authentication
- Role-Based Authorization
- Global Exception Handling
- Serilog Logging

📄 **[Architecture Documentation](docs/architecture.md)**

---

## Technologies

| Category                 | Technologies                                                     |
| ------------------------ | ---------------------------------------------------------------- |
| **Language**             | C#                                                               |
| **Framework**            | ASP.NET Core Web API, .NET 10                                    |
| **Database**             | SQL Server, Entity Framework Core                                |
| **Authentication**       | JWT, Refresh Tokens, Role-Based Authorization                    |
| **Validation & Logging** | FluentValidation, Serilog                                        |
| **File Storage**         | Cloudinary                                                       |
| **API Documentation**    | Swagger / OpenAPI                                                |
| **Testing**              | xUnit, Moq, FluentAssertions, AutoFixture, WebApplicationFactory |

📄 **[Full Tech Stack Details](docs/technologies.md)**

---

## API Endpoints

| Module         | Endpoints |
| -------------- | --------: |
| Authentication |         4 |
| Users          |         4 |
| Companies      |         3 |
| Candidates     |         6 |
| Jobs           |        10 |
| Applications   |         5 |
| Interviews     |         5 |
| Offers         |         5 |
| Notifications  |         4 |
| Reviews        |         5 |
| Uploads        |         1 |
| Admin          |        12 |
| **Total**      |    **64** |

📄 **[Complete API Documentation](docs/api.md)**

---

## Database

- 14+ Entities
- SQL Server
- Entity Framework Core (Code First)

📄 **[Database Schema Documentation](docs/database.md)**

---

## Testing

| Metric      |     Value |
| ----------- | --------: |
| **Tests**   |    **86** |
| **Passed**  | **86** ✅ |
| **Failed**  |     **0** |
| **Skipped** |     **0** |

📄 **[Testing Documentation](docs/testing.md)**

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Quick Setup

```bash
# 1. Clone the repository
git clone https://github.com/devAhmed28/SmartHire.git
cd SmartHire

# 2. Configure the connection string and local secrets

# 3. Apply migrations
dotnet ef database update --project src/SmartHire.Infrastructure --startup-project src/SmartHire.API

# 4. Run the application
dotnet run --project src/SmartHire.API

# 5. Open the Swagger URL shown in the terminal
```

---

## Security

Sensitive credentials should not be committed to Git.

For local development:

- Store secrets using .NET User Secrets or environment variables.
- Keep real JWT and Cloudinary credentials out of source control.
- Use safe placeholder values in committed configuration files.

📄 **[Security Documentation](docs/security.md)**

---

## Documentation

More detailed information about the project is available in the `docs/` directory.

| Document                                           | Description                |
| -------------------------------------------------- | -------------------------- |
| [Features](docs/features.md)                       | Complete feature list      |
| [Architecture](docs/architecture.md)               | Architecture details       |
| [Technologies](docs/technologies.md)               | Tech stack details         |
| [API](docs/api.md)                                 | Complete API documentation |
| [Database](docs/database.md)                       | Database schema            |
| [Authentication](docs/authentication.md)           | Authentication details     |
| [Testing](docs/testing.md)                         | Testing guide              |
| [Security](docs/security.md)                       | Security and secrets       |
| [Development Roadmap](docs/development-roadmap.md) | Project roadmap            |

---

## Project Status

The core recruitment backend has been implemented and tested.

### ✅ Completed

- Core recruitment features
- Authentication and authorization
- Database and persistence layer
- Unit tests
- Integration tests
- API tests
- Swagger/OpenAPI documentation

### 🚧 Future Improvements

| Feature                         | Status     |
| ------------------------------- | ---------- |
| Docker Containerization         | ⏳ Planned |
| Redis Caching                   | ⏳ Planned |
| SignalR Real-time Communication | ⏳ Planned |
| GitHub Actions CI/CD            | ⏳ Planned |
| Cloud Deployment                | ⏳ Planned |

These features are intentionally planned for later versions of the project.

---

## Author

**Ahmed Rabie**

- 📧 Email: `devea7med@gmail.com`
- 🐙 GitHub: [@devAhmed28](https://github.com/devAhmed28)

---

## License

This project is licensed under the MIT License.

---

**Made with ❤️ by Ahmed Rabie**
