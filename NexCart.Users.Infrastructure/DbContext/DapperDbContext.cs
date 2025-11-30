using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace NexCart.Users.Infrastructure.DbContext;

public class DapperDbContext : IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
        _connection = new SqlConnection(connectionString);
    }

    public IDbConnection Connection => _connection;

    public void Dispose()
    {
        _connection?.Dispose();
    }
}





