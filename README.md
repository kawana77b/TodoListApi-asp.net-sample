# TodoApi ASP.NET sample

This is a simple example of the technologies

- ASP.NET
- Entity Framework
- Identity Core
- JWT Authentication

**This is only a sample for MY learning purposes**, and the database will be stored in the local root `app.db`.  
In other words, this repository is a perfectly scratch.

## In Visual Studio

Whether local or container execution, debugging first requires some work.

To use it in Visual Studio, it must first be migrated in the package manager.
From **the Package Manager console, make TodoApi your default project and do the following.**

```pwsh
Update-Database
```

(as a reminder in case I clone this repository again) Work can begin here.

## with docker

I don't have a detailed setup, but I can get it up and running outside of Visual Studio.
If you are not familiar with .NET, use the following command:

```bash
docker compose up -d
```

**This is migrated internally and does not require any special work as described in the previous section.**

The database is maintained in a container.

Since it is set to development mode, you can view Swagger at the following address However, no detailed settings have been made.

`http://localhost:3000/swagger/`

If someone other than me tries this, the first endpoint for user registration is as follows:

`http://localhost:3000/api/identity/register`

An access token will be distributed so you can try the API with it.
