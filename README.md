# Factoring Service

This project is a C# / .NET Web API built to calculate the receivables factoring of a comapany's invoices.

More information about requirements can be found here: https://github.com/pratesy/size-tecnico.

## Installation and Setup

### Requirements

- This project uses .NET 9 and SQL Server

- A database backup is provided in the file db-backup.bak

### Setup

- After cloning the repo/downloading the code, configure the file `src\ReceivablesFactoring.WebApi\appsettings.json` with your database instance and credentials information

- Then run the commands (inside the project's root folder)

```
dotnet build
```

- You can run unit tests with
```
dotnet test
```

- And finally, to startup the Web API project:

```
dotnet run --project .\src\ReceivablesFactoring.WebApi\ReceivablesFactoring.WebApi.csproj
```

- After that, the API should be running at `http://localhost:5267/`

### Usage

- The endpoints documentation is available at `http://localhost:5267/swagger/index.html`

- You can create a new company at `/api/v1/Company` and use its CNPJ to get a bearer token at `/api/v1/Company/auth/{cnpj}`

- With the bearer token, you can create invoices at `/api/v1/Invoice` and add/remove them or calculate the `Factoring` endpoints
