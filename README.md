# Real Estate Listing API - Screening Project for Senior C# Developers

## Overview
This Real Estate Listing API serves as a baseline project for assessing the skills of candidates applying for the Senior C# Developer position. It utilizes .NET 6 with an in-memory database managed by Entity Framework Core, and is documented using Swagger.

## Purpose
The project is designed to evaluate candidates' proficiency in enhancing an existing API framework by adhering to best practices in coding, implementing design patterns, and ensuring the application's scalability and maintainability.

## Technical Specifications
- **Framework:** .NET 8
- **Database:** In-Memory Database (Entity Framework Core)
- **API Documentation:** Swagger/OpenAPI

## Project Setup
To run this project locally:

1. Install the .NET SDK on your machine.
2. Clone the repository to your preferred location.
3. Open a terminal at the project's root directory.
4. Run the following commands:
   ```bash
   dotnet restore
   dotnet build
   dotnet run `

1.  Navigate to `http://localhost:(5236 or 7289)/swagger` in your browser to interact with the API using Swagger UI.

Candidate Enhancement Tasks
---------------------------

As a candidate for the Senior C# Developer position, you are expected to expand upon the base project with the following implementations:

### Input Validations

-   **Enhance Input Validation:** Ensure all incoming data via API endpoints meet predefined formats and constraints. Implement robust validation logic to prevent invalid data submissions.

### Design Patterns

-   **Dependency Injection (DI):** Utilize DI extensively to decouple the application's dependencies. Enhance the current setup by refining service registrations and their usages throughout the application.
-   **Repository Pattern:** Implement the Repository Pattern to abstract data access logic into reusable classes. This should help isolate the data layer, making the system easier to maintain and test.
-   **Unit of Work:** Incorporate the Unit of Work pattern to manage transactional operations. This ensures that operations involving multiple repositories are done within a single transaction context.a

### CRUD Operations

-   **Implement DELETE Functionality:** Add a DELETE endpoint to allow users to remove listings from the memory. Ensure that this operation adheres to RESTful standards and includes appropriate validation and error handling to manage the integrity of the database.

### Dockerization

-   **Containerize the Application:** Dockerize the application to ensure it can run in a containerized environment. This includes creating a `Dockerfile` and possibly a `docker-compose.yml` if necessary, to define how the application should be built and run in Docker.


Contribution Guidelines
-----------------------

-   **Code Quality:** Write clean, scalable, and readable code that adheres to common C# coding standards.
-   **Testing:** Include unit tests for new features to ensure reliability and help prevent future regressions.
-   **Documentation:** Update documentation and comments as necessary to keep them relevant to the code changes.
