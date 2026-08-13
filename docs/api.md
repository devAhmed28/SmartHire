# SmartHire API

This document describes the HTTP API exposed by the SmartHire backend.

The API is organized into modules based on the main features of the recruitment platform.

---

## API Overview

The main API modules are:

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

The API uses standard HTTP methods:

| Method   | Purpose                             |
| -------- | ----------------------------------- |
| `GET`    | Retrieve data                       |
| `POST`   | Create data or perform an operation |
| `PUT`    | Update data                         |
| `DELETE` | Remove data                         |

Protected endpoints require authentication using a JWT Bearer access token.

---

## Authentication

Authentication endpoints are provided through the `AuthController`.

### Register

```text
POST /api/auth/register
```

Registers a new user.

Supported registration roles include:

- Candidate
- Company

### Login

```text
POST /api/auth/login
```

Authenticates a user and returns authentication tokens.

### Refresh Token

```text
POST /api/auth/refresh
```

Creates a new access token using a valid refresh token.

### Logout

```text
POST /api/auth/logout
```

Logs out the authenticated user.

---

## Users

User profile endpoints are provided through the `UsersController`.

### Get Current User

```text
GET /api/users/me
```

Returns the profile of the currently authenticated user.

### Update Current User

```text
PUT /api/users/me
```

Updates the profile information of the currently authenticated user.

### Change Password

```text
PUT /api/users/me/change-password
```

Changes the password of the currently authenticated user.

### Upload Profile Image

```text
POST /api/users/me/profile-image
```

Uploads or updates the profile image of the current user.

---

## Companies

Company profile endpoints are provided through the company controller.

### Get Company Profile

```text
GET /api/companies/me
```

Returns the profile of the currently authenticated company.

### Update Company Profile

```text
PUT /api/companies/me
```

Updates the profile information of the currently authenticated company.

### Upload Company Logo

```text
POST /api/companies/me/logo
```

Uploads or updates the company's logo.

---

### Authorization

Company and user endpoints require authentication.

Company-specific operations are restricted using role-based authorization.

## Candidates

Candidate endpoints are provided through the candidate-related controllers.

### Get Candidate Profile

```text
GET /api/candidates/me
```

Returns the profile of the currently authenticated candidate.

### Update Candidate Profile

```text
PUT /api/candidates/me
```

Updates the current candidate's profile.

### Upload CV

```text
POST /api/candidates/me/cv
```

Uploads a CV for the currently authenticated candidate.

### Add Skill

```text
POST /api/candidates/skills
```

Adds a skill to the current candidate's profile.

### Remove Skill

```text
DELETE /api/candidates/skills/{skillName}
```

Removes a skill from the current candidate's profile.

### Get Saved Jobs

```text
GET /api/candidates/me/saved-jobs
```

Returns the jobs saved by the current candidate.

Candidate endpoints require authentication and are restricted to the Candidate role where applicable.

---

## Jobs

Job endpoints are provided through the `JobsController`.

### Create Job

```text
POST /api/jobs
```

Creates a new job posting.

### Get Company Jobs

```text
GET /api/jobs/company
```

Returns jobs belonging to the authenticated company.

### Get Job

```text
GET /api/jobs/{id}
```

Returns details for a specific job.

### Update Job

```text
PUT /api/jobs/{id}
```

Updates an existing job.

### Delete Job

```text
DELETE /api/jobs/{id}
```

Deletes a job.

### Search Jobs

```text
GET /api/jobs/search
```

Searches and filters available jobs.

### Publish Job

```text
POST /api/jobs/{id}/publish
```

Publishes a job.

### Close Job

```text
POST /api/jobs/{id}/close
```

Closes a job.

### Save Job

```text
POST /api/jobs/{jobId}/save
```

Saves a job for the authenticated candidate.

### Unsave Job

```text
DELETE /api/jobs/{jobId}/save
```

Removes a job from the authenticated candidate's saved jobs.

Job operations use authentication and role-based authorization where required.

---

## Applications

Application endpoints are provided through the applications controller.

### Apply for a Job

```text
POST /api/applications
```

Creates an application for a job for the authenticated candidate.

### Get My Applications

```text
GET /api/applications/me
```

Returns the applications belonging to the authenticated candidate.

### Get Job Applications

```text
GET /api/applications/job/{jobId}
```

Returns applications submitted for a specific company job.

### Update Application Status

```text
PUT /api/applications/{id}/status
```

Updates the status of an application.

This operation is available to authorized company users.

### Withdraw Application

```text
DELETE /api/applications/{id}
```

Withdraws an application submitted by the candidate.

---

## Interviews

Interview endpoints are provided through the interviews controller.

### Schedule Interview

```text
POST /api/interviews
```

Schedules an interview as part of the recruitment process.

### Get My Interviews

```text
GET /api/interviews/me
```

Returns interviews associated with the authenticated user.

### Get Company Interviews

```text
GET /api/interviews/company
```

Returns interviews associated with the authenticated company.

### Get Interview

```text
GET /api/interviews/{id}
```

Returns details for a specific interview.

### Update Interview Status

```text
PUT /api/interviews/{id}/status
```

Updates the status of an interview.

Interview endpoints require authentication and use role-based authorization where applicable.

---

## Offers

Offer endpoints are provided through the offers controller.

### Send Offer

```text
POST /api/offers
```

Creates and sends an offer to a candidate.

### Get My Offers

```text
GET /api/offers/me
```

Returns offers associated with the authenticated candidate.

### Get Company Offers

```text
GET /api/offers/company
```

Returns offers created by the authenticated company.

### Accept Offer

```text
POST /api/offers/{id}/accept
```

Accepts an offer.

### Reject Offer

```text
POST /api/offers/{id}/reject
```

Rejects an offer.

---

## Notifications

Notification endpoints are provided through the notifications controller.

### Get My Notifications

```text
GET /api/notifications/me
```

Returns notifications for the authenticated user.

### Get Unread Count

```text
GET /api/notifications/unread-count
```

Returns the number of unread notifications.

### Mark Notification as Read

```text
PUT /api/notifications/{id}/read
```

Marks a specific notification as read.

### Mark All as Read

```text
PUT /api/notifications/read-all
```

Marks all notifications belonging to the authenticated user as read.

Notification endpoints require authentication.

---

## Reviews

Review endpoints are provided through the reviews controller.

### Create Review

```text
POST /api/reviews
```

Creates a review for a company.

### Get Company Reviews

```text
GET /api/reviews/company/{companyId}
```

Returns reviews for a specific company.

### Get My Reviews

```text
GET /api/reviews/me
```

Returns reviews created by the authenticated user.

### Update Review

```text
PUT /api/reviews/{id}
```

Updates an existing review.

### Delete Review

```text
DELETE /api/reviews/{id}
```

Deletes an existing review.

---

## File Uploads

SmartHire uses Cloudinary for file storage.

### Upload Candidate CV

```text
POST /api/candidates/me/cv
```

Uploads a CV for the authenticated candidate.

### Upload Profile Image

```text
POST /api/users/me/profile-image
```

Uploads a profile image for the authenticated user.

### Upload Company Logo

```text
POST /api/companies/me/logo
```

Uploads a logo for the authenticated company.

### Delete File

```text
DELETE /api/uploads
```

Deletes an uploaded file.

File upload endpoints require authentication and use role-based authorization where applicable.

---

## Administration

Administrative endpoints are available to users with the Admin role.

### Dashboard

```text
GET /api/admin/dashboard
```

Returns statistics and summary information for the platform.

### Get Companies

```text
GET /api/admin/companies
```

Returns the companies managed by the administrator.

### Get Company Details

```text
GET /api/admin/companies/{id}
```

Returns details for a specific company.

### Verify Company

```text
PUT /api/admin/companies/{id}/verify
```

Verifies a company.

### Deactivate Company

```text
PUT /api/admin/companies/{id}/deactivate
```

Deactivates a company.

### Get Users

```text
GET /api/admin/users
```

Returns users managed by the administrator.

### Get User Details

```text
GET /api/admin/users/{id}
```

Returns details for a specific user.

### Deactivate User

```text
PUT /api/admin/users/{id}/deactivate
```

Deactivates a user.

### Activate User

```text
PUT /api/admin/users/{id}/activate
```

Activates a user.

### Get Jobs

```text
GET /api/admin/jobs
```

Returns jobs available for administrative management.

### Get Job Details

```text
GET /api/admin/jobs/{id}
```

Returns details for a specific job.

### Delete Job

```text
DELETE /api/admin/jobs/{id}
```

Deletes a job as part of administrative job moderation.

All administration endpoints require authentication and the **Admin** role.

---

## API Responses

SmartHire uses standardized application results to provide consistent responses from the API.

Depending on the operation, an endpoint can return:

- Successful responses
- Validation errors
- Not found errors
- Conflict errors
- Unauthorized responses
- Forbidden responses
- Internal server errors

### Common HTTP Status Codes

| Status Code                 | Meaning                                    |
| --------------------------- | ------------------------------------------ |
| `200 OK`                    | Request completed successfully             |
| `201 Created`               | Resource was created successfully          |
| `204 No Content`            | Request completed without a response body  |
| `400 Bad Request`           | Request validation or input error          |
| `401 Unauthorized`          | Authentication is required or invalid      |
| `403 Forbidden`             | User is authenticated but not authorized   |
| `404 Not Found`             | Requested resource was not found           |
| `409 Conflict`              | Operation conflicts with the current state |
| `500 Internal Server Error` | Unexpected server error                    |

---

## Authentication

Protected endpoints require a valid JWT Bearer token.

The token should be sent using the HTTP `Authorization` header:

```text
Authorization: Bearer {access_token}
```

Swagger can be used during development to provide the token and test protected endpoints.

---

## API Documentation

Swagger / OpenAPI provides an interactive interface for exploring and testing the SmartHire API.

When the application is running, open the Swagger URL displayed by the application.

The Swagger interface can be used to:

- View available endpoints
- View HTTP methods
- Inspect request models
- Inspect response models
- Authorize using a JWT token
- Send API requests during development

---
