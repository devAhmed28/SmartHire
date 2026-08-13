# SmartHire Database

This document describes the database structure used by the SmartHire backend.

SmartHire uses **SQL Server** with **Entity Framework Core** and follows the **Code First** approach.

---

## Database Overview

The database stores the main information required for the recruitment workflow.

The main areas of the database are:

- Users and authentication
- Companies
- Candidate profiles
- Jobs
- Job applications
- Interviews
- Offers
- Reviews
- Saved jobs
- Notifications
- Skills
- Refresh tokens

Entity Framework Core is responsible for mapping the application entities to database tables and handling database persistence.

Database schema changes are managed using **Entity Framework Core migrations**.

---

## Main Entities

The current application contains entities including:

| Entity             | Purpose                                    |
| ------------------ | ------------------------------------------ |
| `User`             | Stores user and authentication information |
| `Company`          | Stores company information                 |
| `CandidateProfile` | Stores candidate profile information       |
| `Job`              | Stores job postings                        |
| `JobApplication`   | Stores applications submitted for jobs     |
| `Interview`        | Stores interview information               |
| `Offer`            | Stores job offers                          |
| `Review`           | Stores company reviews                     |
| `SavedJob`         | Stores jobs saved by candidates            |
| `Notification`     | Stores user notifications                  |
| `RefreshToken`     | Stores refresh token information           |
| `Skill`            | Stores available skills                    |
| `JobSkill`         | Connects jobs with skills                  |
| `CandidateSkill`   | Connects candidates with skills            |

---

## Entity Relationships

The database represents relationships between the main recruitment entities.

For example:

```text
User
 ├── Company
 ├── CandidateProfile
 ├── RefreshToken
 ├── Notification
 └── Review

Company
 └── Job
      ├── JobApplication
      ├── Interview
      └── Offer

CandidateProfile
 ├── CandidateSkill
 ├── JobApplication
 ├── SavedJob
 ├── Interview
 └── Offer

Job
 ├── JobSkill
 ├── JobApplication
 └── SavedJob
```

The exact relationship configuration and constraints are defined through the Entity Framework Core model configuration in the Infrastructure layer.

---

## Persistence

Database access is handled by the Infrastructure layer.

The main persistence technologies are:

- SQL Server
- Entity Framework Core
- Entity Framework Core migrations
- Repository Pattern
- Unit of Work

The Application layer communicates with persistence through abstractions, while Infrastructure contains the concrete database implementations.

---

## Relationships

The main relationships between the entities are organized around users, companies, candidates, jobs, and the recruitment process.

### User Relationships

A `User` can be associated with:

- A `Company`
- A `CandidateProfile`
- Multiple `RefreshToken` records
- Multiple `Notification` records
- Reviews created by the user

### Company Relationships

A `Company` can have:

- Multiple `Job` records
- Company profile information
- Company-related reviews

### Candidate Relationships

A `CandidateProfile` can have:

- Multiple skills through `CandidateSkill`
- Multiple job applications
- Multiple saved jobs
- Candidate-related interviews
- Candidate-related offers

### Job Relationships

A `Job` can have:

- Multiple required skills through `JobSkill`
- Multiple applications
- Multiple saved-job records
- Recruitment-related interviews and offers

### Many-to-Many Relationships

Skills are connected to candidates and jobs through separate relationship entities:

```text
CandidateProfile
       ↓
CandidateSkill
       ↓
     Skill

Job
 ↓
JobSkill
 ↓
Skill
```

This allows a candidate to have multiple skills and a job to require multiple skills.

---

## Recruitment Flow

The database supports the main recruitment workflow:

```text
Company
   ↓
Create Job
   ↓
Candidate
   ↓
Apply for Job
   ↓
JobApplication
   ↓
Interview
   ↓
Offer
   ↓
Accept / Reject
```

Additional entities such as `Notification`, `Review`, and `SavedJob` support related platform functionality.

---

## Entity Details

### User

The `User` entity represents an account in the system.

It is used for:

- Authentication
- Authorization
- User profile information
- Connecting users to company or candidate information
- Refresh tokens
- Notifications
- Reviews

---

### Company

The `Company` entity represents a company using the recruitment platform.

It contains company-related information and is associated with the jobs created by the company.

Companies can also have administrative states such as verification and activation status.

---

### CandidateProfile

The `CandidateProfile` entity contains information specific to candidates.

It is associated with:

- Candidate skills
- Job applications
- Saved jobs
- Interviews
- Offers

---

### Job

The `Job` entity represents a job posting created by a company.

Jobs can have:

- Job information
- Required skills
- Applications
- Saved-job records
- Recruitment-related information
- Publishing and closing states

---

### JobApplication

The `JobApplication` entity represents a candidate's application for a job.

It connects:

```text
Candidate
     ↓
JobApplication
     ↓
Job
```

Applications also contain application status information used during the recruitment process.

---

### Interview

The `Interview` entity represents an interview associated with a recruitment process.

It connects the relevant candidate and company/job application information and contains the interview status and scheduling information.

---

### Offer

The `Offer` entity represents an employment offer made to a candidate.

An offer is associated with the recruitment process and can be accepted or rejected by the candidate.

---

### Review

The `Review` entity represents a review submitted for a company.

Users can create, update, view, and delete their reviews according to the application's authorization rules.

---

### SavedJob

The `SavedJob` entity represents a job saved by a candidate.

It connects a candidate with a job they want to keep for later.

---

### Notification

The `Notification` entity stores notifications associated with users.

Notifications support:

- Reading notifications
- Counting unread notifications
- Marking individual notifications as read
- Marking all notifications as read

---

### RefreshToken

The `RefreshToken` entity stores refresh-token information used by the authentication system.

Refresh tokens allow authenticated users to obtain new access tokens without logging in again.

---

### Skill

The `Skill` entity represents a skill that can be associated with candidates and jobs.

Skills are connected through:

- `CandidateSkill`
- `JobSkill`

---

### CandidateSkill

`CandidateSkill` connects a candidate profile with a skill.

This represents the skills possessed by a candidate.

---

### JobSkill

`JobSkill` connects a job with a skill.

This represents the skills required or associated with a job.

---

## Migrations

SmartHire uses **Entity Framework Core migrations** to manage changes to the database schema.

Migrations allow the database structure to be updated as the application model changes.

### Apply Migrations

From the solution root, run:

```bash
dotnet ef database update --project src/SmartHire.Infrastructure --startup-project src/SmartHire.API
```

This applies the available migrations to the configured SQL Server database.

---

## Database Configuration

The database connection is configured through the application's configuration system.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SmartHireDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

The actual connection string should be configured locally and should not contain sensitive production credentials in source control.

---

## Database Development Workflow

A typical database development workflow is:

```text
Modify Entity / Configuration
          ↓
Create EF Core Migration
          ↓
Review Migration
          ↓
Apply Migration
          ↓
SQL Server Database Updated
```

The database is accessed through Entity Framework Core and the persistence abstractions implemented in the Infrastructure layer.

---

## Summary

The SmartHire database is designed around the main recruitment workflow:

```text
Users
  ↓
Companies / Candidates
  ↓
Jobs
  ↓
Applications
  ↓
Interviews
  ↓
Offers
```

Supporting entities provide authentication, notifications, reviews, saved jobs, and skill management.

The database uses SQL Server with Entity Framework Core Code First and migrations.
