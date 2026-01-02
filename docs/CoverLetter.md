## Project Management API – Architecture & Implementation Notes

This project is a Project Management RESTful API implemented with ASP.NET Core.
The main goal of this project is to demonstrate clean architecture, rich domain modeling, and proper separation of
concerns, rather than focusing only on basic CRUD operations.

---

## Project Overview

The API supports managing projects and their lifecycle, including:

* Creating and updating projects
* Assigning a team to a project
* Starting and ending a project
* Changing project deadlines
* Soft deleting projects
* Retrieving projects (single and list with pagination)

The implementation emphasizes business rules inside the domain and a clear application flow using commands and queries.

---

## Architectural Approach

### Rich Domain Model

The core of the system is built around a rich domain model:

* `Project` is implemented as an Aggregate Root
* Business rules such as:

    * preventing invalid state transitions
    * handling soft deletion
    * controlling project lifecycle
      are enforced inside the domain entity
* No business logic is placed inside controllers, repositories, or handlers

This keeps the domain consistent and independent from infrastructure concerns.

---

### CQRS (Command & Query Separation)

The application follows the CQRS pattern:

* Commands handle state changes (Create, Update, Start, End, Delete, Assign Team, Change Deadline)
* Queries handle read-only operations (Get by id, Get all with pagination)
* Each use case has:

    * its own Command or Query
    * a dedicated Handler
    * a corresponding Validator

Controllers are thin and only dispatch commands and queries through MediatR.

---

### Repository Pattern

Repositories act as a boundary between the domain and the data access layer:

* Repository interfaces are defined in the Domain layer
* They expose only the behaviors required by the domain (Add, Get, Update, Remove)
* The domain has no dependency on EF Core or `DbContext`

EF Core–based implementations are placed in the Infrastructure layer, keeping persistence details isolated.

---

### Unit of Work

A **Unit of Work** is used to manage database transactions:

* Repositories do not call `SaveChanges`
* All changes are tracked during a request
* `UnitOfWork.CommitAsync()` determines when changes are persisted

This approach ensures consistency and keeps handlers focused on coordination, not persistence details.

---

### Interceptors (Cross-Cutting Concerns)

Two dedicated interceptors handle cross-cutting concerns globally:

* **Audit Interceptor**

    * Automatically sets `CreatedAt` and `ModifiedAt`
    * Removes repetitive audit-related logic from handlers and entities

* **Soft Delete Interceptor**

    * Converts delete operations into soft deletes
    * Ensures soft-deleted entities are filtered out consistently

This keeps repositories and handlers clean and predictable.

---

### Validation Strategy

Validation responsibilities are intentionally split:

* DTOs use Data Annotations for basic input validation
* Commands and Queries use FluentValidation for application-level and business rules

This avoids mixing API concerns with domain or application logic.

---

### Exception Handling

A centralized ExceptionHandlerMiddleware is used to:

* Catch domain and application exceptions
* Map them to meaningful HTTP responses
* Keep controllers and handlers free of try/catch logic

---

## Implemented APIs

### Command-based APIs

* `CreateProject`
* `UpdateProject`
* `DeleteProject`
* `AssignTeamProject`
* `ChangeProjectDeadline`
* `StartProject`
* `EndProject`

### Query-based APIs

* `GetProjectById`
  Retrieves a single project by its identifier.

* `GetAllProjects`
  Retrieves a paginated list of projects with filtering support.

---

## Final Notes

This project is designed to be maintainable, extensible, and close to real-world backend practices.

It can be extended with:

* Authentication and authorization
* Additional domain rules
* More advanced reporting queries
* Event-driven integrations

The focus throughout the implementation was on clear boundaries, correct domain behavior, and architectural discipline.

---

This document was drafted with the assistance of an AI tool to improve clarity and structure;
however, the project itself was fully designed and implemented based on our own knowledge, experience, and architectural
decisions.