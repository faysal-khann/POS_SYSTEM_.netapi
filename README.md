# POS System

A Point of Sale (POS) system for managing sales, products, inventory, customers, suppliers, and business operations.

## Projects

- **Backend:** .NET 10 Web API
- **Frontend:** React Native with Expo
- **Database:** Azure SQL Database

**Frontend repository:** [POS System Frontend](https://github.com/faysal-khann/POS-SYSTEM.git)

## Features

- User login and management
- Products, categories, brands, and units
- Customer and supplier management
- Sales and sales returns
- Purchases and inventory management
- Loyalty points
- Expenses and cashier shifts
- Product image and company logo uploads

## Technologies

- C# and ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- SQL Server / Azure SQL Database
- JWT Authentication
- Swagger / OpenAPI
- Clean Architecture

## Project Structure

```text
POS_SYSTEM/
├── POS_SYSTEM.Api/
├── POS_SYSTEM.Application/
├── POS_SYSTEM.Domain/
└── POS_SYSTEM.Infrastructure/
```

## Getting Started

### Requirements

- .NET 10 SDK
- SQL Server or Azure SQL Database

### Run Locally

1. Clone the repository.
2. Open the solution.
3. Configure the database connection string and JWT secret.
4. Restore dependencies and run the API:

```bash
dotnet restore
dotnet run --project POS_SYSTEM.Api
```

Run the API in Development mode to access Swagger at:

```text
/swagger
```

## Deployment

The backend API and database are hosted on Microsoft Azure.

## Note

Keep database credentials and JWT secrets private. Do not commit real credentials to the repository.
