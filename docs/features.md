# SmartHire Features

This document describes the main features currently implemented in the SmartHire backend.

---

## Authentication

SmartHire provides authentication for the main user types in the system.

### Supported Operations

- Candidate registration
- Company registration
- Login
- JWT access token generation
- Refresh token support
- Logout
- Role-based authorization

### User Roles

The application supports the following roles:

- **Candidate**
- **Company**
- **Admin**

Authentication and authorization are handled through JWT Bearer Authentication and role-based access control.

---

## User Management

SmartHire provides basic user profile management for authenticated users.

### Supported Operations

- Get the current user's profile
- Update the current user's profile
- Change password
- Upload profile image

User operations are protected by authentication, so users can manage their own profile information.

---

## Company Management

Companies have their own profile and management features.

### Supported Operations

- Get company profile
- Update company profile
- Upload company logo
- Company verification by Admin
- Company activation and deactivation by Admin

Company-specific operations are protected by role-based authorization so that company functionality is available only to the appropriate users.

---

## Job Management

Companies can create and manage job postings through the recruitment workflow.

### Supported Operations

- Create jobs
- Update jobs
- Delete jobs
- Get job details
- Get company jobs
- Search and filter jobs
- Publish jobs
- Close jobs
- Save jobs
- Unsave jobs

Job operations use role-based authorization to ensure that company-specific actions are performed by authorized users.

---

## Candidate Features

Candidates can manage their profiles and participate in the recruitment process.

### Supported Operations

- Get candidate profile
- Update candidate profile
- Add skills
- Remove skills
- Upload CV
- Apply for jobs
- Withdraw applications
- View saved jobs

Candidate-specific operations are protected by role-based authorization.

---

## Applications

Candidates can apply for published jobs and manage their applications throughout the recruitment process.

### Supported Operations

- Apply for a job
- View my applications
- View applications for a company job
- Update application status
- Withdraw an application

Application status can be updated by authorized company users as the application moves through the recruitment process.

---

## Interviews

Companies can schedule and manage interviews as part of the recruitment process.

### Supported Operations

- Schedule an interview
- View candidate interviews
- View company interviews
- Get interview details
- Update interview status

Interview operations are protected by role-based authorization to ensure that users can access only the interviews relevant to them.

---

## Offers

Companies can send offers to candidates as part of the recruitment process.

### Supported Operations

- Send an offer
- View candidate offers
- View company offers
- Accept an offer
- Reject an offer

Offer operations are protected by role-based authorization so that companies and candidates can perform only the actions relevant to their roles.

---

## Notifications

SmartHire provides in-app notifications to keep users informed about relevant activity.

### Supported Operations

- Get notifications
- Get unread notification count
- Mark a notification as read
- Mark all notifications as read

Notifications are associated with users and can be managed through authenticated endpoints.

---

## Reviews

Users can create and manage reviews for companies based on their experience.

### Supported Operations

- Create a review
- View company reviews
- View my reviews
- Update a review
- Delete a review

Review operations are protected by authentication and authorization to ensure that users can manage only their own reviews.

---

## File Uploads

SmartHire supports file uploads for different parts of the recruitment platform.

### Supported Operations

- Upload candidate CV
- Upload profile image
- Upload company logo
- Delete uploaded files
- Store uploaded files using Cloudinary

File upload operations are protected by authentication and role-based authorization where required.

---

## Admin

Administrators have access to management and moderation features across the platform.

### Dashboard

- View platform statistics

### User Management

- View users
- View user details
- Activate users
- Deactivate users

### Company Management

- View companies
- View company details
- Verify companies
- Deactivate companies

### Job Management

- View jobs
- View job details
- Delete jobs
- Moderate job postings

Admin operations are protected using role-based authorization and are available only to users with the Admin role.

---
