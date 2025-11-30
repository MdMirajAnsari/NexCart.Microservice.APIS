# Database Migration Guide

## Overview
This project has been migrated from using multiple databases (PostgreSQL for Users, MySQL for Products) to a **single SQL Server LocalDB instance** with a unified **NexCart** database.

## Changes Made

### 1. Database Provider Changes

#### Users API
- **Before:** PostgreSQL with Npgsql
- **After:** SQL Server with Microsoft.Data.SqlClient
- **Connection String Key:** Changed from `PostgresConnection` to `DefaultConnection`

#### Products API
- **Before:** MySQL with MySql.EntityFrameworkCore
- **After:** SQL Server with Microsoft.EntityFrameworkCore.SqlServer
- **Connection String Key:** Already using `DefaultConnection`

#### Orders API
- **Current:** Still using MongoDB (no changes)
- **Future:** Can be migrated to SQL Server if needed

### 2. NuGet Package Changes

#### NexCart.Users.Infrastructure.csproj
```xml
<!-- Removed -->
<PackageReference Include="Npgsql" Version="9.0.3" />

<!-- Added -->
<PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.0" />
```

#### NexCart.Products.Context.csproj
```xml
<!-- Removed -->
<PackageReference Include="MySql.Data" Version="9.3.0" />
<PackageReference Include="MySql.Data.EntityFrameworkCore" Version="8.0.22" />

<!-- Added -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
```

### 3. Code Changes

#### Users API - DapperDbContext.cs
```csharp
// Before
using Npgsql;
string connectionString = _configuration.GetConnectionString("PostgresConnection")!;
_connection = new NpgsqlConnection(connectionString);

// After
using Microsoft.Data.SqlClient;
string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
_connection = new SqlConnection(connectionString);
```

#### Products API - DADependencyInjection.cs
```csharp
// Before
services.AddDbContext<ApplicationDbContext>(options => 
    options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));

// After
services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
```

### 4. Connection String Configuration

All `appsettings.json` files now use:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NexCart;Integrated Security=true;TrustServerCertificate=true"
  }
}
```

## Migration Steps

### Step 1: Delete Old Migrations (If Any)
If you have existing migrations from PostgreSQL or MySQL, you need to delete them:

```bash
# For Users API
cd NexCart.UsersApi
rm -r Migrations  # PowerShell: Remove-Item -Recurse -Force Migrations

# For Products API
cd ../NexCart.ProductsApi
rm -r Migrations  # PowerShell: Remove-Item -Recurse -Force Migrations
```

### Step 2: Create New Migrations for SQL Server

```bash
# For Users API
cd NexCart.UsersApi
dotnet ef migrations add InitialCreate
dotnet ef database update

# For Products API
cd ../NexCart.ProductsApi
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 3: Verify Database Creation

You can verify the database was created using:

1. **Visual Studio 2022:**
   - Open **SQL Server Object Explorer** (View > SQL Server Object Explorer)
   - Expand **(localdb)\MSSQLLocalDB**
   - Look for the **NexCart** database

2. **SQL Server Management Studio (SSMS):**
   - Connect to: `(localdb)\MSSQLLocalDB`
   - Browse databases for **NexCart**

3. **Command Line:**
   ```bash
   SqlLocalDB info MSSQLLocalDB
   ```

## Benefits of This Change

✅ **Simplified Setup:** No need to install PostgreSQL or MySQL  
✅ **Included with Visual Studio:** SQL Server LocalDB comes with VS 2022  
✅ **Single Database:** All microservices use the same database instance  
✅ **Windows Authentication:** No password management needed  
✅ **Easy Development:** Quick to set up and tear down  
✅ **Schema Separation:** Each service can still maintain separate tables/schemas  

## Database Schema Separation

Even though all services use the same database, they maintain logical separation:

- **Users tables:** `AspNetUsers`, `AspNetRoles`, etc.
- **Products tables:** `Products`, `Categories`, etc.
- **Orders tables:** `Orders`, `OrderItems`, etc.

You can optionally use **SQL Server Schemas** for better separation:
```sql
CREATE SCHEMA Users;
CREATE SCHEMA Products;
CREATE SCHEMA Orders;
```

## Troubleshooting

### Issue: LocalDB not found
**Solution:** Install SQL Server LocalDB
```bash
# Download from: https://aka.ms/ssedt
# Or install with Visual Studio Installer
```

### Issue: Cannot connect to (localdb)\MSSQLLocalDB
**Solution:** Start LocalDB instance
```bash
SqlLocalDB start MSSQLLocalDB
```

### Issue: Database already exists with old schema
**Solution:** Drop and recreate database
```bash
SqlLocalDB stop MSSQLLocalDB
SqlLocalDB delete MSSQLLocalDB
SqlLocalDB create MSSQLLocalDB
SqlLocalDB start MSSQLLocalDB
```

## Reverting to Original Configuration

If you need to revert to PostgreSQL/MySQL:

1. Restore the old NuGet packages
2. Restore the old connection strings in appsettings.json
3. Update the code to use PostgreSQL/MySQL providers
4. Delete SQL Server migrations
5. Restore old migrations or create new ones

---

**Last Updated:** 2025-11-30
