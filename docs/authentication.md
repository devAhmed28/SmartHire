# SmartHire Authentication

This document describes the authentication and authorization flow used by the SmartHire backend.

---

## Authentication Overview

SmartHire uses **JWT Bearer Authentication** for securing API endpoints.

The authentication system supports:

- User registration
- Login
- JWT access tokens
- Refresh tokens
- Logout
- Role-based authorization

The main user roles are:

- Candidate
- Company
- Admin

---

## Authentication Flow

The general authentication flow is:

```text
User
 ↓
Register / Login
 ↓
Authentication Handler
 ↓
Access Token + Refresh Token
 ↓
Client
 ↓
Protected API Request
 ↓
JWT Authentication
 ↓
Role Authorization
 ↓
Controller
```

The access token is used to authenticate requests to protected endpoints.

The refresh token can be used to obtain a new access token when the current access token expires.

---

## Registration

New users can register through:

```text
POST /api/auth/register
```

Registration supports the main user types required by the application:

- Candidate
- Company

The registration process validates the request before creating the user account.

---

## Login

Users can authenticate through:

```text
POST /api/auth/login
```

A successful login returns authentication tokens that can be used to access protected API endpoints.

---

## JWT Access Tokens

SmartHire uses JWT access tokens to authenticate API requests.

A protected request includes the token in the HTTP `Authorization` header:

```text
Authorization: Bearer {access_token}
```

The API validates the token before allowing access to protected endpoints.

---

## Refresh Tokens

Refresh tokens are used to obtain a new access token after the current access token expires.

The refresh operation is available through:

```text
POST /api/auth/refresh
```

This allows users to continue their authenticated session without logging in again.

---

## Logout

Authenticated users can log out through:

```text
POST /api/auth/logout
```

Logout handles the authentication state according to the application's token management implementation.

---

## Authorization

Authentication answers:

> Who is the user?

Authorization answers:

> Is this user allowed to perform this operation?

SmartHire uses role-based authorization for protected operations.

### Candidate

Candidates can access candidate-specific functionality such as:

- Candidate profile
- Skills
- CV uploads
- Job applications
- Saved jobs
- Candidate interviews
- Candidate offers

### Company

Companies can access company-specific functionality such as:

- Company profile
- Company logo
- Job management
- Job applications
- Interviews
- Offers

### Admin

Administrators can access platform management functionality such as:

- User management
- Company management
- Company verification
- Job moderation
- Dashboard statistics

---

## Protected Endpoints

Protected endpoints require a valid JWT access token.

Some endpoints also require a specific role.

The authorization flow is:

```text
HTTP Request
     ↓
JWT Authentication
     ↓
Is the token valid?
     ↓
Role Authorization
     ↓
Is the user allowed?
     ↓
Controller
```

If authentication fails, the API returns an unauthorized response.

If authentication succeeds but the user does not have the required role, the API returns a forbidden response.

---

## Security

Authentication credentials and secrets should not be committed to source control.

Sensitive configuration includes:

- JWT secret
- Database connection credentials
- Cloudinary API credentials

For local development, sensitive values should be stored using:

- .NET User Secrets
- Environment variables
- Local configuration that is excluded from Git

Production deployments should use an appropriate secret-management solution.

---

## Authentication Configuration

Authentication is configured through the ASP.NET Core configuration system.

The main JWT settings include:

- Secret key
- Issuer
- Audience
- Access token expiration
- Refresh token expiration

Example structure:

```json
{
  "JwtSettings": {
    "Secret": "your-secret-key",
    "Issuer": "SmartHire",
    "Audience": "SmartHireClients",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

The values above are examples only. Real secrets should not be committed to the repository.

---

## Token Lifetime

SmartHire uses separate lifetimes for access tokens and refresh tokens.

The current configuration uses:

- Access token: 15 minutes
- Refresh token: 7 days

These values can be changed through the application's configuration.

---

## Security Considerations

JWT secrets and other sensitive credentials must be kept outside source control.

For local development, use:

- .NET User Secrets
- Environment variables
- Local configuration excluded by `.gitignore`

For production environments, use a proper secret-management solution.

The repository should contain only safe placeholder values when configuration examples are committed.

---

## Authentication Endpoints

The authentication endpoints are exposed through the authentication controller.

### Register

```text
POST /api/auth/register
```

Creates a new Candidate or Company account.

### Login

```text
POST /api/auth/login
```

Authenticates a user and returns the authentication tokens.

### Refresh Token

```text
POST /api/auth/refresh
```

Uses a valid refresh token to obtain a new access token.

### Logout

```text
POST /api/auth/logout
```

Logs out the authenticated user.

---

## Authentication Errors

Common authentication-related responses include:

| Status Code        | Meaning                                            |
| ------------------ | -------------------------------------------------- |
| `400 Bad Request`  | Invalid request or validation failure              |
| `401 Unauthorized` | Missing or invalid authentication                  |
| `403 Forbidden`    | Authenticated user does not have the required role |

Authentication and authorization errors are handled through the API's standard response and exception-handling mechanisms.

---

## Authentication Security

SmartHire separates authentication secrets from the application source code.

### Secrets

The following values should never contain real production credentials in committed files:

- JWT secret
- Database credentials
- Cloudinary API secret
- Other private API credentials

For local development, use:

- .NET User Secrets
- Environment variables

### Password Security

User passwords should be handled through the application's authentication and identity mechanisms rather than stored as plain text.

### Token Security

Access tokens should be treated as sensitive credentials.

Clients should:

- Send access tokens only over HTTPS
- Avoid exposing tokens in logs
- Avoid committing tokens to source control
- Request a new access token through the refresh-token flow when required

### Production Considerations

For production deployments:

- Use HTTPS
- Store secrets in a secure secret-management system
- Use appropriate token expiration periods
- Avoid logging sensitive authentication information
- Keep production credentials outside the repository

---
