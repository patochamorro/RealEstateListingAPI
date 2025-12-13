# Real Estate Listing API Challenge

## Overview

Real Estate Listing REST API built with ASP.NET Core 8.0, exposing CRUD endpoints for property listings with input validation, logging, unit-of-work/repository patterns, SQL Server persistence, and async cancellation-token support.



## Swagger

* Remotely deployed on Heroku: [API Swagger](https://agile-hamlet-06201-50c5817e684a.herokuapp.com/swagger/index.html)
* Local: [API Swagger](http://localhost:8080/swagger/index.html)



## Budgets

[![CircleCI](https://dl.circleci.com/status-badge/img/gh/patochamorro/RealEstateListingAPI/tree/main.svg?style=svg)](https://dl.circleci.com/status-badge/redirect/gh/patochamorro/RealEstateListingAPI/tree/main)
[![Coverage Status](https://coveralls.io/repos/github/patochamorro/RealEstateListingAPI/badge.svg)](https://coveralls.io/github/patochamorro/RealEstateListingAPI)



## Project structure

* `RealEstateListingApi/` – Web API project

  * `Presentation/Controllers/` – API endpoints (Listings)
  * `Application/` – DTOs, validators, services, exceptions, interfaces
  * `Domain/` – Entities and repository contracts
  * `Infrastructure/` – DbContext, EF migrations, repository/unit-of-work implementations, middleware
  * Config: Program.cs, appsettings*.json
* `RealEstateListingApi.Tests/` – xUnit tests

  * `Application/` – Unit tests
  * `Integration/` – Integration tests
* Root tooling: docker-compose.yml (API + DB), global.json (SDK pin), README.md



## Features
* **CancellationToken** lets you stop work early (like database calls or long requests) when the client disconnects or a timeout happens. It avoids wasted work and frees resources, keeping the API responsive under load.
* **Validation**

  * Logical uniqueness enforced at the **API level** for `Title + Address`
  * Single generic validator: `ListingInputValidator<T>`
  * All input DTOs inherit from a common base DTO
  * Performs field-level validation
  * **Custom validation** to ensure `Title + Address` uniqueness via repository query

* **Persistence**

  * EF Core with SQL Server provider

* **Data access patterns**

  * Database connection pooling defined in the connection string in `appsettings.json`
  * Repository pattern
  * Unit of Work pattern

* **Error Handling**

  * Custom `ServiceException` differentiating **business** and **technical** errors
  * Centralized exception middleware that logs one-line entries using the format:

    ```
    Service | Endpoint | CorrelationId | Status | Message | StackTrace
    ```

* **Docker / Docker Compose**

  * `Dockerfile` for the ASP.NET Core API
  * `docker-compose.yml` for local/dev:

    * API container
    * **SQL Server container**

* **Testing**

  * Project `RealEstateListingApi.Tests` using **xUnit**
  * Application tests
  * Integration tests

* **Deployment**

  * Deployed on [Heroku](https://agile-hamlet-06201-50c5817e684a.herokuapp.com/swagger/index.html)



## Pre-Requisites

* Docker installed without sudo permissions
* Docker Compose installed without sudo
* Free ports: 8080 and 1433



## How to run the app locally

```bash
chmod 711 ./up_dev.sh
./up_dev.sh
```



## How to run tests + coverage

```bash
chmod 711 ./up_test.sh
./up_test.sh
```

If you want to view code coverage, you can find the report at:
`./RealEstateListingApi.Tests/TestResults/coveragereport/index.html`



## Areas to improve

* Uniqueness enforcement is **not guaranteed at the DB level**
* Potential race conditions under high concurrency
* For production-grade systems, adding a DB constraint would be strongly recommended
* Secure the API using OAuth2, JWT, API keys, etc.
* Manage API versioning



## Techs

Technologies used across the solution:

* Platform: .NET 8 / ASP.NET Core Web API
* Data: Entity Framework Core (Npgsql/PostgreSQL), with earlier SQL Server migrations present
* Validation: FluentValidation
* API Docs: Swagger / Swashbuckle
* Testing: xUnit
* Tooling: Docker & docker-compose, Heroku Procfile
* Logging/Errors: Custom middleware with structured logs, custom `ServiceException`
* Build: global.json pinning the .NET SDK (8.0.x)
* CI/CD: CircleCI pipeline (assumes `dotnet build` / `dotnet test`)
* Coverage: Coveralls integration



## Decisions made

* **Clean Architecture:** Keeps each microservice’s core logic simple and independent, allowing technology changes without rewrites, fast testing, and preventing codebase degradation as it grows.
* **Entity Framework:** Maps C# classes to the database, builds queries automatically, and handles migrations. EF uses `DbContext` as a Unit of Work, ensuring changes are saved together in a single transaction.
* **Docker:** Used to ensure portability.
* **xUnit:** Lightweight, fast in CI, and integrates smoothly with ASP.NET Core and test runners.

