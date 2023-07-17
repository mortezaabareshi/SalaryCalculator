using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Salaries.Infrastructure.DataAccess.Dapper;

public class DapperDbContext
{
    private readonly string _connectionString;
    public DapperDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Postgres");
    }
    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}