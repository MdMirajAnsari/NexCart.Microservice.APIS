# Stored Procedure Architecture Guide

## Overview
This guide demonstrates a clean architecture approach for calling SQL Server stored procedures using Dapper, processing the data, and performing business operations.

## Architecture Layers

```
┌─────────────────────────────────────────────────────────────┐
│                    API Layer (Controllers)                   │
│  - Receives HTTP Requests                                   │
│  - Returns HTTP Responses                                   │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                   Service Layer (Business Logic)            │
│  - Orchestrates operations                                  │
│  - Applies business rules                                   │
│  - Transforms DTOs                                          │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│              Repository Layer (Data Access)                 │
│  - Calls stored procedures                                  │
│  - Maps data to entities                                    │
│  - No business logic                                        │
└────────────────────────┬────────────────────────────────────┘
                         │
┌────────────────────────▼────────────────────────────────────┐
│                SQL Server Database                          │
│  - Stored Procedures                                        │
│  - Tables                                                   │
└─────────────────────────────────────────────────────────────┘
```

---

## Project Structure

```
NexCart.Users/
├── NexCart.Users.Entities/           # Domain entities
│   └── ApplicationUser.cs
├── NexCart.Users.DTO/                # Data Transfer Objects
│   ├── UserRequest.cs
│   └── UserResponse.cs
├── NexCart.Users.RepositoryContracts/ # Interfaces
│   └── IUsersRepository.cs
├── NexCart.Users.Infrastructure/     # Data Access Implementation
│   ├── DbContext/
│   │   └── DapperDbContext.cs
│   └── Repositories/
│       └── UserRepository.cs
├── NexCart.Users.ServiceContracts/   # Service Interfaces
│   └── IUsersService.cs
└── NexCart.Users.Services/           # Business Logic
    └── UsersService.cs
```

---

## Step-by-Step Implementation

### 1. Create Stored Procedures in SQL Server

```sql
-- Example: Get User By ID with additional calculations
CREATE PROCEDURE sp_GetUserById
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        UserId,
        Email,
        PersonName,
        Gender,
        CreatedDate,
        LastLoginDate
    FROM Users
    WHERE UserId = @UserId AND IsActive = 1;
END
GO

-- Example: Search Users with Filters
CREATE PROCEDURE sp_SearchUsers
    @SearchTerm NVARCHAR(100) = NULL,
    @Gender NVARCHAR(10) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Get total count
    SELECT @TotalCount = COUNT(*)
    FROM Users
    WHERE (@SearchTerm IS NULL OR PersonName LIKE '%' + @SearchTerm + '%' OR Email LIKE '%' + @SearchTerm + '%')
    AND (@Gender IS NULL OR Gender = @Gender)
    AND IsActive = 1;
    
    -- Get paginated results
    SELECT 
        UserId,
        Email,
        PersonName,
        Gender,
        CreatedDate
    FROM Users
    WHERE (@SearchTerm IS NULL OR PersonName LIKE '%' + @SearchTerm + '%' OR Email LIKE '%' + @SearchTerm + '%')
    AND (@Gender IS NULL OR Gender = @Gender)
    AND IsActive = 1
    ORDER BY CreatedDate DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

-- Example: Create User with Business Logic
CREATE PROCEDURE sp_CreateUser
    @UserId UNIQUEIDENTIFIER OUTPUT,
    @Email NVARCHAR(255),
    @PersonName NVARCHAR(100),
    @Gender NVARCHAR(10),
    @Password NVARCHAR(255),
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if email already exists
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        BEGIN
            SET @ResultCode = -1;
            SET @ResultMessage = 'Email already exists';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Generate new UserId
        SET @UserId = NEWID();
        
        -- Insert user
        INSERT INTO Users (UserId, Email, PersonName, Gender, Password, CreatedDate, IsActive)
        VALUES (@UserId, @Email, @PersonName, @Gender, @Password, GETDATE(), 1);
        
        SET @ResultCode = 0;
        SET @ResultMessage = 'User created successfully';
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @ResultCode = ERROR_NUMBER();
        SET @ResultMessage = ERROR_MESSAGE();
    END CATCH
END
GO

-- Example: Update User with Audit Trail
CREATE PROCEDURE sp_UpdateUser
    @UserId UNIQUEIDENTIFIER,
    @PersonName NVARCHAR(100),
    @Gender NVARCHAR(10),
    @ModifiedBy UNIQUEIDENTIFIER,
    @ResultCode INT OUTPUT,
    @ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM Users WHERE UserId = @UserId)
        BEGIN
            SET @ResultCode = -1;
            SET @ResultMessage = 'User not found';
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Update user
        UPDATE Users
        SET PersonName = @PersonName,
            Gender = @Gender,
            ModifiedDate = GETDATE(),
            ModifiedBy = @ModifiedBy
        WHERE UserId = @UserId;
        
        -- Log to audit table
        INSERT INTO UserAuditLog (UserId, Action, ModifiedBy, ModifiedDate)
        VALUES (@UserId, 'UPDATE', @ModifiedBy, GETDATE());
        
        SET @ResultCode = 0;
        SET @ResultMessage = 'User updated successfully';
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SET @ResultCode = ERROR_NUMBER();
        SET @ResultMessage = ERROR_MESSAGE();
    END CATCH
END
GO
```

---

### 2. Entity Layer (Domain Models)

**NexCart.Users.Entities/ApplicationUser.cs**
```csharp
namespace NexCart.Users.Entities;

public class ApplicationUser
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PersonName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsActive { get; set; }
}
```

---

### 3. DTO Layer (Data Transfer Objects)

**NexCart.Users.DTO/UserSearchRequest.cs**
```csharp
namespace NexCart.Users.DTO;

public class UserSearchRequest
{
    public string? SearchTerm { get; set; }
    public string? Gender { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
```

**NexCart.Users.DTO/UserSearchResponse.cs**
```csharp
namespace NexCart.Users.DTO;

public class UserSearchResponse
{
    public List<UserDTO> Users { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
```

**NexCart.Users.DTO/StoredProcedureResult.cs**
```csharp
namespace NexCart.Users.DTO;

public class StoredProcedureResult<T>
{
    public int ResultCode { get; set; }
    public string ResultMessage { get; set; } = string.Empty;
    public T? Data { get; set; }
    public bool IsSuccess => ResultCode == 0;
}
```

---

### 4. Repository Contract (Interface)

**NexCart.Users.RepositoryContracts/IUsersRepository.cs**
```csharp
using NexCart.Users.Entities;
using NexCart.Users.DTO;

namespace NexCart.Users.RepositoryContracts;

public interface IUsersRepository
{
    // Simple SP call
    Task<ApplicationUser?> GetUserById(Guid userId);
    
    // SP with multiple result sets
    Task<UserSearchResponse> SearchUsers(UserSearchRequest request);
    
    // SP with output parameters
    Task<StoredProcedureResult<ApplicationUser>> CreateUser(ApplicationUser user);
    
    // SP with transaction handling
    Task<StoredProcedureResult<bool>> UpdateUser(ApplicationUser user, Guid modifiedBy);
    
    // SP returning scalar value
    Task<int> GetActiveUserCount();
    
    // SP with complex mapping
    Task<List<ApplicationUser>> GetUsersByFilter(Dictionary<string, object> filters);
}
```

---

### 5. Repository Implementation (Data Access)

**NexCart.Users.Infrastructure/Repositories/UserRepository.cs**
```csharp
using Dapper;
using NexCart.Users.Entities;
using NexCart.Users.DTO;
using NexCart.Users.Infrastructure.DbContext;
using NexCart.Users.RepositoryContracts;
using System.Data;

namespace NexCart.Users.Infrastructure.Repositories;

public class UserRepository : IUsersRepository
{
    private readonly DapperDbContext _dbContext;
    
    public UserRepository(DapperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // ============================================
    // EXAMPLE 1: Simple Stored Procedure Call
    // ============================================
    public async Task<ApplicationUser?> GetUserById(Guid userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", userId, DbType.Guid);

        var user = await _dbContext.Connection.QueryFirstOrDefaultAsync<ApplicationUser>(
            "sp_GetUserById",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return user;
    }

    // ============================================
    // EXAMPLE 2: SP with Output Parameters
    // ============================================
    public async Task<StoredProcedureResult<ApplicationUser>> CreateUser(ApplicationUser user)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", dbType: DbType.Guid, direction: ParameterDirection.Output);
        parameters.Add("@Email", user.Email, DbType.String);
        parameters.Add("@PersonName", user.PersonName, DbType.String);
        parameters.Add("@Gender", user.Gender, DbType.String);
        parameters.Add("@Password", user.Password, DbType.String);
        parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ResultMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

        await _dbContext.Connection.ExecuteAsync(
            "sp_CreateUser",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var resultCode = parameters.Get<int>("@ResultCode");
        var resultMessage = parameters.Get<string>("@ResultMessage");
        var userId = parameters.Get<Guid>("@UserId");

        var result = new StoredProcedureResult<ApplicationUser>
        {
            ResultCode = resultCode,
            ResultMessage = resultMessage,
            Data = resultCode == 0 ? new ApplicationUser { UserId = userId, Email = user.Email, PersonName = user.PersonName, Gender = user.Gender } : null
        };

        return result;
    }

    // ============================================
    // EXAMPLE 3: SP with Multiple Result Sets
    // ============================================
    public async Task<UserSearchResponse> SearchUsers(UserSearchRequest request)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SearchTerm", request.SearchTerm, DbType.String);
        parameters.Add("@Gender", request.Gender, DbType.String);
        parameters.Add("@PageNumber", request.PageNumber, DbType.Int32);
        parameters.Add("@PageSize", request.PageSize, DbType.Int32);
        parameters.Add("@TotalCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

        var users = await _dbContext.Connection.QueryAsync<ApplicationUser>(
            "sp_SearchUsers",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var totalCount = parameters.Get<int>("@TotalCount");

        return new UserSearchResponse
        {
            Users = users.Select(u => new UserDTO
            {
                UserId = u.UserId,
                Email = u.Email,
                PersonName = u.PersonName,
                Gender = u.Gender
            }).ToList(),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    // ============================================
    // EXAMPLE 4: SP with Transaction Handling
    // ============================================
    public async Task<StoredProcedureResult<bool>> UpdateUser(ApplicationUser user, Guid modifiedBy)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@UserId", user.UserId, DbType.Guid);
        parameters.Add("@PersonName", user.PersonName, DbType.String);
        parameters.Add("@Gender", user.Gender, DbType.String);
        parameters.Add("@ModifiedBy", modifiedBy, DbType.Guid);
        parameters.Add("@ResultCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@ResultMessage", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

        await _dbContext.Connection.ExecuteAsync(
            "sp_UpdateUser",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        var resultCode = parameters.Get<int>("@ResultCode");
        var resultMessage = parameters.Get<string>("@ResultMessage");

        return new StoredProcedureResult<bool>
        {
            ResultCode = resultCode,
            ResultMessage = resultMessage,
            Data = resultCode == 0
        };
    }

    // ============================================
    // EXAMPLE 5: SP Returning Scalar Value
    // ============================================
    public async Task<int> GetActiveUserCount()
    {
        var count = await _dbContext.Connection.ExecuteScalarAsync<int>(
            "sp_GetActiveUserCount",
            commandType: CommandType.StoredProcedure
        );

        return count;
    }

    // ============================================
    // EXAMPLE 6: Dynamic Parameters from Dictionary
    // ============================================
    public async Task<List<ApplicationUser>> GetUsersByFilter(Dictionary<string, object> filters)
    {
        var parameters = new DynamicParameters();
        
        foreach (var filter in filters)
        {
            parameters.Add($"@{filter.Key}", filter.Value);
        }

        var users = await _dbContext.Connection.QueryAsync<ApplicationUser>(
            "sp_GetUsersByFilter",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return users.ToList();
    }
}
```

---

### 6. Service Contract (Interface)

**NexCart.Users.ServiceContracts/IUsersService.cs**
```csharp
using NexCart.Users.DTO;

namespace NexCart.Users.ServiceContracts;

public interface IUsersService
{
    Task<UserDTO?> GetUserById(Guid userId);
    Task<UserSearchResponse> SearchUsers(UserSearchRequest request);
    Task<ServiceResult<UserDTO>> CreateUser(CreateUserRequest request);
    Task<ServiceResult<bool>> UpdateUser(Guid userId, UpdateUserRequest request);
    Task<int> GetActiveUserCount();
}
```

---

### 7. Service Implementation (Business Logic)

**NexCart.Users.Services/UsersService.cs**
```csharp
using AutoMapper;
using NexCart.Users.DTO;
using NexCart.Users.Entities;
using NexCart.Users.RepositoryContracts;
using NexCart.Users.ServiceContracts;

namespace NexCart.Users.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;
    private readonly IMapper _mapper;

    public UsersService(IUsersRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // ============================================
    // Simple Operation: Get User
    // ============================================
    public async Task<UserDTO?> GetUserById(Guid userId)
    {
        // Call repository (which calls SP)
        var user = await _repository.GetUserById(userId);
        
        if (user == null)
            return null;

        // Perform business operations on data
        var userDto = _mapper.Map<UserDTO>(user);
        
        // Additional business logic (e.g., calculate age, format data, etc.)
        userDto.DisplayName = FormatDisplayName(user.PersonName, user.Gender);
        
        return userDto;
    }

    // ============================================
    // Complex Operation: Search with Validation
    // ============================================
    public async Task<UserSearchResponse> SearchUsers(UserSearchRequest request)
    {
        // Business validation
        if (request.PageSize > 100)
            request.PageSize = 100; // Max limit
        
        if (request.PageNumber < 1)
            request.PageNumber = 1;

        // Call repository (which calls SP)
        var response = await _repository.SearchUsers(request);
        
        // Post-processing business logic
        foreach (var user in response.Users)
        {
            user.DisplayName = FormatDisplayName(user.PersonName, user.Gender);
            // Mask sensitive data if needed
            user.Email = MaskEmail(user.Email);
        }

        return response;
    }

    // ============================================
    // Create Operation with Business Rules
    // ============================================
    public async Task<ServiceResult<UserDTO>> CreateUser(CreateUserRequest request)
    {
        // Business validation
        if (!IsValidEmail(request.Email))
        {
            return ServiceResult<UserDTO>.Failure("Invalid email format");
        }

        // Hash password (business logic)
        var hashedPassword = HashPassword(request.Password);

        // Map to entity
        var user = new ApplicationUser
        {
            Email = request.Email,
            PersonName = request.PersonName,
            Gender = request.Gender,
            Password = hashedPassword
        };

        // Call repository (which calls SP)
        var spResult = await _repository.CreateUser(user);

        if (!spResult.IsSuccess)
        {
            return ServiceResult<UserDTO>.Failure(spResult.ResultMessage);
        }

        // Additional operations after successful creation
        // e.g., Send welcome email, log activity, etc.
        await SendWelcomeEmail(spResult.Data!.Email);
        
        var userDto = _mapper.Map<UserDTO>(spResult.Data);
        return ServiceResult<UserDTO>.Success(userDto, spResult.ResultMessage);
    }

    // ============================================
    // Update Operation with Audit
    // ============================================
    public async Task<ServiceResult<bool>> UpdateUser(Guid userId, UpdateUserRequest request)
    {
        // Get current user for validation
        var existingUser = await _repository.GetUserById(userId);
        if (existingUser == null)
        {
            return ServiceResult<bool>.Failure("User not found");
        }

        // Business rules: Check if changes are allowed
        if (!CanModifyUser(existingUser))
        {
            return ServiceResult<bool>.Failure("User modification not allowed");
        }

        // Apply changes
        existingUser.PersonName = request.PersonName;
        existingUser.Gender = request.Gender;

        // Call repository (which calls SP)
        var currentUserId = GetCurrentUserId(); // From authentication context
        var spResult = await _repository.UpdateUser(existingUser, currentUserId);

        if (!spResult.IsSuccess)
        {
            return ServiceResult<bool>.Failure(spResult.ResultMessage);
        }

        // Post-update operations
        await LogUserActivity(userId, "USER_UPDATED");

        return ServiceResult<bool>.Success(true, spResult.ResultMessage);
    }

    // ============================================
    // Statistics Operation
    // ============================================
    public async Task<int> GetActiveUserCount()
    {
        return await _repository.GetActiveUserCount();
    }

    // ============================================
    // Private Helper Methods (Business Logic)
    // ============================================
    private string FormatDisplayName(string name, string gender)
    {
        var prefix = gender.ToLower() == "male" ? "Mr." : "Ms.";
        return $"{prefix} {name}";
    }

    private string MaskEmail(string email)
    {
        var parts = email.Split('@');
        if (parts.Length != 2) return email;
        
        var username = parts[0];
        if (username.Length <= 3) return email;
        
        return $"{username.Substring(0, 3)}***@{parts[1]}";
    }

    private bool IsValidEmail(string email)
    {
        // Email validation logic
        return email.Contains("@") && email.Contains(".");
    }

    private string HashPassword(string password)
    {
        // Password hashing logic (use BCrypt, PBKDF2, etc.)
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private async Task SendWelcomeEmail(string email)
    {
        // Email sending logic
        await Task.CompletedTask;
    }

    private bool CanModifyUser(ApplicationUser user)
    {
        // Business rules for modification
        return user.IsActive;
    }

    private Guid GetCurrentUserId()
    {
        // Get from HTTP context / claims
        return Guid.NewGuid(); // Placeholder
    }

    private async Task LogUserActivity(Guid userId, string activity)
    {
        // Activity logging
        await Task.CompletedTask;
    }
}
```

---

### 8. Supporting Classes

**NexCart.Users.DTO/ServiceResult.cs**
```csharp
namespace NexCart.Users.DTO;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ServiceResult<T> Success(T data, string message = "Operation successful")
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }

    public static ServiceResult<T> Failure(string message, List<string>? errors = null)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}
```

---

### 9. Controller Example

**NexCart.UsersApi/Controllers/UsersController.cs**
```csharp
using Microsoft.AspNetCore.Mvc;
using NexCart.Users.DTO;
using NexCart.Users.ServiceContracts;

namespace NexCart.UsersApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var user = await _usersService.GetUserById(userId);
        
        if (user == null)
            return NotFound(new { Message = "User not found" });
        
        return Ok(user);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchUsers([FromBody] UserSearchRequest request)
    {
        var response = await _usersService.SearchUsers(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await _usersService.CreateUser(request);
        
        if (!result.IsSuccess)
            return BadRequest(new { result.Message, result.Errors });
        
        return CreatedAtAction(nameof(GetUser), new { userId = result.Data!.UserId }, result.Data);
    }

    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequest request)
    {
        var result = await _usersService.UpdateUser(userId, request);
        
        if (!result.IsSuccess)
            return BadRequest(new { result.Message });
        
        return Ok(new { result.Message });
    }

    [HttpGet("stats/active-count")]
    public async Task<IActionResult> GetActiveUserCount()
    {
        var count = await _usersService.GetActiveUserCount();
        return Ok(new { ActiveUserCount = count });
    }
}
```

---

## Best Practices

### 1. **Separation of Concerns**
- Repository: Only data access, no business logic
- Service: Business logic, validation, orchestration
- Controller: HTTP handling, routing

### 2. **Error Handling**
```csharp
// In Repository
try
{
    var result = await _dbContext.Connection.QueryAsync<T>(sp, params);
    return result;
}
catch (SqlException ex)
{
    // Log and rethrow or wrap in custom exception
    throw new DataAccessException("Error calling stored procedure", ex);
}
```

### 3. **Connection Management**
```csharp
// Ensure connections are properly disposed
public class DapperDbContext : IDisposable
{
    private readonly IDbConnection _connection;
    
    public IDbConnection Connection => _connection;
    
    public void Dispose()
    {
        _connection?.Dispose();
    }
}
```

### 4. **Parameter Validation**
```csharp
// Always validate before calling SP
if (string.IsNullOrWhiteSpace(email))
    throw new ArgumentException("Email cannot be empty", nameof(email));
```

### 5. **Use Strongly Typed Parameters**
```csharp
// Good
parameters.Add("@UserId", userId, DbType.Guid);

// Avoid
parameters.Add("@UserId", userId); // Type inference can fail
```

---

## Summary

This architecture provides:
- ✅ **Clean separation** between data access and business logic
- ✅ **Testability** - Each layer can be mocked
- ✅ **Maintainability** - Changes in one layer don't affect others
- ✅ **Scalability** - Easy to add new operations
- ✅ **Performance** - Stored procedures are pre-compiled
- ✅ **Security** - Parameterized queries prevent SQL injection

**Flow:**
1. Controller receives HTTP request
2. Controller calls Service method
3. Service applies business logic and validation
4. Service calls Repository method
5. Repository executes stored procedure using Dapper
6. Repository returns data to Service
7. Service performs post-processing
8. Service returns result to Controller
9. Controller returns HTTP response
