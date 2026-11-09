using System.Data;
using Microsoft.Data.SqlClient;

namespace FamilyFeet.Api.Infrastructure;

public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found. Check appsettings.json or user secrets.");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
