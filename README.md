# 🛒 NexCart Microservices API

> A modern e-commerce platform built with microservices architecture using ASP.NET Core

## 📋 Table of Contents
- [Overview](#overview)
- [Architecture](#architecture)
- [Technologies](#technologies)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
  - [Users API](#users-api)
  - [Products API](#products-api)
  - [Orders API](#orders-api)
- [Database Configuration](#database-configuration)
- [Project Structure](#project-structure)
- [License](#license)

## 🎯 Overview

NexCart is a microservices-based e-commerce platform that demonstrates modern software architecture principles. The system is divided into three main microservices:

- **Users API** - Handles user authentication and management
- **Products API** - Manages product catalog and inventory
- **Orders API** - Processes orders and transactions

## 🏗️ Architecture

The project follows Clean Architecture principles with the following layers:

- **API Layer** - RESTful endpoints and minimal APIs
- **Service Layer** - Business logic implementation
- **Repository Layer** - Data access abstraction
- **Infrastructure Layer** - Database context and external services
- **DTO Layer** - Data transfer objects
- **Validators** - FluentValidation for input validation
- **Mappers** - AutoMapper for object mapping

## 🛠️ Technologies

- **.NET 8** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for database access
- **SQL Server LocalDB** - Unified database for all services
- **FluentValidation** - Input validation
- **AutoMapper** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation (Users API)
- **RabbitMQ** - Message broker for inter-service communication

## ✅ Prerequisites

Before running the project, ensure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- **SQL Server LocalDB** (included with Visual Studio 2022)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Rider](https://www.jetbrains.com/rider/) (optional)
- [Git](https://git-scm.com/) for version control

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd NexCart.Microservice.APIS
```

### 2. Configure Database

The project uses **SQL Server LocalDB** with a single database named **NexCart** for all services.

SQL Server LocalDB is automatically installed with Visual Studio 2022. If you need to install it separately, download it from [Microsoft SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).

The database will be created automatically when you run migrations (next step).

### 3. Connection String (Already Configured)

All services are configured to use the same SQL Server LocalDB instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NexCart;Integrated Security=true;TrustServerCertificate=true"
}
```

This connection string:
- Uses **Windows Authentication** (Integrated Security)
- Connects to the **NexCart** database
- No password required (uses your Windows user account)

### 4. Run Database Migrations

Run migrations for each service to create the necessary tables:

```bash
# For Users API
cd NexCart.UsersApi
dotnet ef database update

# For Products API
cd ..
cd NexCart.ProductsApi
dotnet ef database update

# For Orders API (when implemented)
cd ..
cd NexCart.OrdersApi
dotnet ef database update
```

### 5. Build the Solution

```bash
dotnet build NexCart.MicroserviceApis.sln
```

### 6. Run the Services

You can run each service individually:

```bash
# Terminal 1 - Users API
cd NexCart.UsersApi
dotnet run

# Terminal 2 - Products API
cd NexCart.ProductsApi
dotnet run

# Terminal 3 - Orders API
cd NexCart.OrdersApi
dotnet run
```

Or use Visual Studio to run multiple startup projects simultaneously.

## 📚 API Documentation

### Users API

Base URL: `https://localhost:<port>`

#### Swagger Documentation
Access interactive API documentation at: `https://localhost:<port>/swagger`

#### Authentication Endpoints

##### Register User
```http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response:**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "message": "User registered successfully"
}
```

##### Login
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response:**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "message": "Login successful"
}
```

#### User Management Endpoints

##### Get User by ID
```http
GET /api/Users/{userID}
```

**Response:**
```json
{
  "userID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe"
}
```

---

### Products API

Base URL: `https://localhost:<port>`

#### Product Endpoints

##### Get All Products
```http
GET /api/products
```

**Response:**
```json
[
  {
    "productID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "productName": "Laptop",
    "category": "Electronics",
    "price": 999.99,
    "stock": 50
  }
]
```

##### Get Product by ID
```http
GET /api/products/search/product-id/{productID}
```

**Response:**
```json
{
  "productID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "productName": "Laptop",
  "category": "Electronics",
  "price": 999.99,
  "stock": 50
}
```

##### Search Products
```http
GET /api/products/search/{searchString}
```

Searches products by name or category.

**Example:** `GET /api/products/search/laptop`

##### Add Product
```http
POST /api/products
Content-Type: application/json

{
  "productName": "Laptop",
  "category": "Electronics",
  "price": 999.99,
  "stock": 50
}
```

**Response:**
```json
{
  "productID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "productName": "Laptop",
  "category": "Electronics",
  "price": 999.99,
  "stock": 50
}
```

##### Update Product
```http
PUT /api/products
Content-Type: application/json

{
  "productID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "productName": "Gaming Laptop",
  "category": "Electronics",
  "price": 1299.99,
  "stock": 45
}
```

##### Delete Product
```http
DELETE /api/products/{productID}
```

**Response:**
```json
true
```

---

### Orders API

⚠️ **Status:** Currently under development

Base URL: `https://localhost:<port>`

```http
GET /
```

**Response:**
```
Hello World!
```

## 💾 Database Configuration

### SQL Server LocalDB - Unified Database

**Connection Details:**
- **Server:** `(localdb)\MSSQLLocalDB`
- **Database:** `NexCart`
- **Authentication:** Windows Authentication (Integrated Security)
- **Connection String:** `Server=(localdb)\MSSQLLocalDB;Database=NexCart;Integrated Security=true;TrustServerCertificate=true`

**Benefits of using a single database:**
- ✅ Simplified development and testing
- ✅ No need to install PostgreSQL or MySQL
- ✅ Easy data relationships between services (if needed)
- ✅ Included with Visual Studio 2022

**All three microservices (Users, Products, Orders) use the same database but maintain separate schemas/tables.**

### Viewing the Database

You can connect to the database using:
- **SQL Server Object Explorer** in Visual Studio
- **SQL Server Management Studio (SSMS)**
- **Azure Data Studio**

Connection string: `(localdb)\MSSQLLocalDB`

## 📁 Project Structure

```
NexCart.Microservice.APIS/
├── UsersApi/
│   ├── NexCart.UsersApi/              # API Layer
│   ├── NexCart.Users.Services/        # Business Logic
│   ├── NexCart.Users.Infrastructure/  # Data Access
│   ├── NexCart.Users.DTO/            # Data Transfer Objects
│   ├── NexCart.Users.Entities/       # Domain Models
│   ├── NexCart.Users.Validators/     # Input Validation
│   ├── NexCart.Users.Mappers/        # Object Mapping
│   ├── NexCart.Users.ServiceContracts/
│   ├── NexCart.Users.RepositoryContracts/
│   └── NextCart.Users.Helpers/       # Utility Classes
├── ProductsApi/
│   ├── NexCart.ProductsApi/          # API Layer
│   ├── NexCart.Products.Services/    # Business Logic
│   ├── NexCart.Products.Repositories/# Data Access
│   ├── NexCart.Products.Context/     # EF Core Context
│   ├── NexCart.Products.DTO/         # Data Transfer Objects
│   ├── NexCart.Products.Entities/    # Domain Models
│   ├── NexCart.Products.Validators/  # Input Validation
│   ├── NexCart.Products.Mappers/     # Object Mapping
│   ├── NexCart.Products.ServiceContracts/
│   ├── NexCart.Products.RepositoryContracts/
│   └── NexCart.Products.Helpers/     # Utility Classes
├── OrdersApi/
│   ├── NexCart.OrdersApi/            # API Layer
│   ├── NexCart.Orders.Services/      # Business Logic
│   ├── NexCart.Orders.Repositories/  # Data Access
│   ├── NexCart.Orders.DTO/           # Data Transfer Objects
│   ├── NexCart.Orders.Entities/      # Domain Models
│   ├── NexCart.Orders.Validators/    # Input Validation
│   ├── NexCart.Orders.Mappers/       # Object Mapping
│   ├── NexCart.Orders.HttpClients/   # External API Clients
│   ├── NexCart.Orders.Policies/      # Retry & Circuit Breaker
│   ├── NexCart.Orders.ServiceContracts/
│   ├── NexCart.Orders.RepositoryContracts/
│   └── NexCart.Orders.Helpers/       # Utility Classes
├── Infrastructure/
│   └── NexCart.ServiceBus/           # RabbitMQ Integration
└── NexCart.MicroserviceApis.sln      # Solution File
```

## 📄 License

This project is licensed under the terms specified in the LICENSE.txt file.

---

**Built with ❤️ using .NET**
