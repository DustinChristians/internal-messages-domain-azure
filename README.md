# A .NET Core WEB API (developed for Azure) for a messaging application.

# Features
- The [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- A Microsoft SQL Server Local Database with seeded data so you can spin up the project and test it out. See the spin up instructions below.
- Entity Framework Core O/RM with code first migrations.
- A base repository that includes all normal CRUD operations.
- Search methods and extensions for easy searching within repositories.
- Support for bulk database operations like BulkCreate, BulkUpdate and BulkDelete thanks to the free [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) package.
- A RESTful API that includes an endpoint for messages, users and project settings. The messages and users tables are used for setting up project patterns and can be removed if they aren't needed.
- An Event Log powered by Serilog. Events are logged to the database and the file system as a backup.
- NUnit tests for the existing messages, users and settings repositories.

## Localhost Spinup Instructions
- Clone this repository and open it in Visual Studio 2019.
- If you aren't on the latest update for Visual Studio 2019 then you should perform the update to get .NET Core 3.1. In the toolbar, Go to Help -> Check for updates -> Update.
- Right click the **WebApi** project and select **Set as Startup Project**.
- The database is a localdb so once the project is run for the first time the local database will be created using Entity Framework code-first migrations and seeded with test data. 
- Upon startup you will see the seeded **message** data in your browser.

## Projects

Projects are setup to conform to the [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) guidelines.

**Internal.Messages.WebApi**
A RESTful Web Api project. 

**Internal.Messages.Core**
A core project to hold domain models, aggregates, interfaces, constants etc.

**Internal.Messages.Infrastructure**
A project for services and business logic.

**Internal.Messages.Configuration**
Configurations for Dependency Injection, AutoMapper and Entity Framework Core to be shared across projects.

**Internal.Messages.Repository**
This repository uses Entity Framework Core and the Repository Pattern for CRUD operations.

Running Code First Migrations:
1. Open the **Package Manager Console** window in Visual Studio.
2. Set the Default project at the top of the console window to: `Internal.Messages.Repository`
3. Run the command: `Add-Migration NameOfYourMigration --verbose -startupproject Internal.Messages.WebApi`

**Internal.Messages.Tests**
A Unit testing project.

**Internal.Messages.TestUtilities**
TestUtilities project for sharing logging, in memory database dependencies and AutoMapper mocking between test projects.  
 
## Linting with StyleCop
To add StyleCop to a project in the solution, install the StyleCop.Analyzers Nuget Package.
Then, add the custom ruleset to the .csproj file with this code snippet (you may need to adjust the path):

    <PropertyGroup>
    	<CodeAnalysisRuleSet>../CustomStyleCopRules.ruleset</CodeAnalysisRuleSet>
    </PropertyGroup>
