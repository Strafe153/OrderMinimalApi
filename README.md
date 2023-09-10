# OrderMinimalApi
### An ASP.NET Core Minimal Api with MongoDB serving as the database.

## What Can It Do?
* CRUD operations with the Order entity

## Dependencies
* `AspNetCore.HealthChecks.MongoDb` for MongoDB health check
* `AspNetCore.HealthChecks.Redis` for Redis health checks
* `AspNetCore.HealthChecks.UI.Client` for detailed health checks information
* `AutoFixture` for test fixtures
* `AutoFixture.AutoFakeItEasy` for FakeItEasy support with AutoFixture
* `Bogus` for fake data generation
* `FakeItEasy` for mocking
* `FluentAssertions` for assertions
* `FluentValidation` for DTO validation
* `FluentValidation.AspNetCore` for ASP.NET Core integration with FluentValidation
* `Mapster` for DTO mapping
* `Microsoft.AspNetCore.Mvc.NewtonsoftJson` for JSON serialization
* `Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer` for API versioning
* `Microsoft.AspNetCore.OpenApi` for Open API support
* `Microsoft.Extensions.Caching.StackExchangeRedis` for redis cache
* `Microsoft.NET.Test.Sdk` for .NET SDK for testing
* `MongoDB.Driver` for MongoDB
* `Polly.Extensions.Http` for transient fault handling
* `Serilog.AspNetCore` for Serilog support for ASP.NET Core
* `Serilog.Exceptions` for detailed Serilog exceptions
* `Serilog.Sinks.Console` for Serilog console sink
* `Serilog.Sinks.Seq` for Serilog Seq sink
* `Swashbuckle.AspNetCore` for Swagger support
* `xunit` for unit-tests
* `xunit.runner.visualstudio` for running tests in Visual Studio
