# SparkUpSolution

> A .NET Solution mimicking an online casino's backend. To be looked at for interview purposes.

## ⚙️ Features

- **Bonus management and fetching**
- **Rule enforcement:**
  - Only one active bonus of each type per player
- **Audit logging** (Operations store operator info & timestamps)
- **JWT Authentication** (Bearer tokens)
- **Global exception handling**
- **Development runtime seeding** using Bogus
- **Async pipeline**
- **Unit tests**
- **EF Core**

---

## 🏛️ Architecture Overview

The solution follows a simplified layered architecture with some naming conventions for different areas of the application which deal with separate concerns. Below are the directories with some brief descriptions of their contents:

- **src**

  - **Application** (DTOs, Request models, AutoMapper profiles, Services with business logic)
  - **Controllers** (API controllers)
  - **Domain** (Pure business objects representing our domain)
  - **Extensions** (Utilities for different types)
  - **Infrastructure** (Logic handling data and core manipulation)
    - Logging (Audit models and services)
    - Persistence (Migrations and database context)
    - Repositories
  - **Middlewares**

- **test**

  - **SparkUpTests** (Test project containing different tests)
    - Unit

---

## 💻 Setup Instructions

- Make sure you have **Postgres** installed
- Create a login role
- Edit or use the provided `appsettings.Development.json` to use the login from before

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=sparkup;Username=sparkup;Password=sparkup;"
}
```

- Run `dotnet ef database update` from the `src` directory to apply migrations and create the database
- Set `SparkUpSolution.csproj` as the startup project and run it
- Application is set up so that it seeds data when ran in development

```csharp
if (app.Environment.IsDevelopment())
{
    await appDbContext.Seed();
}
```

- To authorize send a request to `POST /login` with the following:

```json
{
  "username": "sparkup",
  "password": "sparkup"
}
```

- Use the token from the response to authorize further requests with Bearer authorization
  - Set `Bearer {TOKEN}` to Swagger's Authorize or add the same as an Authorization header to your requests
