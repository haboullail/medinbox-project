# 📦 Medinbox Equipment Management App

This full stack project was developed as part of the Medinbox technical test.  
It is a layered web application for managing medical equipment, featuring real-time updates and role-based access control.  
The focus was on clean code, scalability, and maintainability.

---

## Project Overview

The backend is built following a **Clean Architecture** pattern, with well-separated concerns:

- **Domain layer**: core business logic and entities
- **Application layer**: interfaces, DTOs, and services
- **Infrastructure layer**: EF Core persistence and data access
- **WebAPI layer**: REST API with JWT authentication and middleware

The frontend is built with Angular + PrimeNG, and a lightweight WebSocket server in Node.js handles real-time communication.

---

## Technology Stack

| Layer      | Technology             | Version     | Reason |
|------------|------------------------|-------------|--------|
| Frontend   | Angular + PrimeNG      | Angular 17  | Latest stable version, SSR-ready, PrimeNG compatible |
| Backend    | ASP.NET Core + EF Core | .NET 8      | LTS version, high performance, clean architecture support |
| Realtime   | Node.js + ws (WebSocket) | Node.js 18 | LTS version, lightweight and decoupled |
| Database   | MySQL                  | 8           | Stable, Docker-compatible |
| Container  | Docker + Docker Compose | —          | Consistent environment and easy orchestration |

---

## Features

### Add Equipment
- Fields: **Name**, **Location**, **Status**
- Form with validation
- Write permission required
- Data saved to DB
- Broadcast to all clients in real-time

### View Equipment List
- Table view with:
  - Name, Location, Status
  - Creation date (newest first)
  - Pagination (5 items per page)
- Read permission required

### Real-Time Updates
- Custom Node.js WebSocket server
- New equipment is pushed instantly to connected clients

### Role & Permissions System
- On login, a **random role** is assigned to the user:
  - **Reader** (read-only)
  - **Writer** (write-only)
  - **ReaderWriter** (read & write)
- Role affects UI and API access
- Role badge is styled with a dedicated color

---

## Project Structure

