using Dapper;
using Salaries.Core.Entities;
using Salaries.Core.Repositories;
using Salaries.Core.ValueObjects;
using Salaries.Infrastructure.DataAccess.Dapper.Conversions;

namespace Salaries.Infrastructure.DataAccess.Dapper.Repositories;

public class SalaryDapperRepository : ISalaryReadRepository
{
    private readonly DapperDbContext _dapperDbContext;

    public SalaryDapperRepository(DapperDbContext dapperDbContext)
    {
        _dapperDbContext = dapperDbContext;
    }

    public async Task<Salary?> GetAsync(Guid id)
    {
        using var connection = _dapperDbContext.CreateConnection();
        connection.Open();
        const string query = "SELECT * FROM \"salaries\".\"Salaries\"  WHERE \"Id\" = @Id AND \"IsDeleted\" = false";
        return await connection
            .QuerySingleOrDefaultAsync<Salary>(query, 
                new { Id = id });
    }

    public async Task<Salary?> GetAsync(Employee employee, DateTime salaryDate)
    {
        using var connection = _dapperDbContext.CreateConnection();
        connection.Open();
        const string query = "SELECT * FROM \"salaries\".\"Salaries\" WHERE \"Employee\" = @Employee " +
                             "AND \"SalaryDate\" = @SalaryDate AND \"IsDeleted\" = false";

        SqlMapper.AddTypeHandler(new EmployeeConversion());

        return await connection
            .QuerySingleOrDefaultAsync<Salary>(query, 
                new { Employee = employee.ToString(), SalaryDate = salaryDate });
    }

    public async Task<IEnumerable<Salary>> GetRangeAsync(Employee employee, DateTime from, DateTime to)
    {
        using var connection = _dapperDbContext.CreateConnection();
        connection.Open();
        const string query = "SELECT * FROM \"salaries\".\"Salaries\" WHERE \"Employee\" = @Employee AND \"SalaryDate\" <= @To " +
                             "AND \"SalaryDate\" >= @From AND \"IsDeleted\" = false";
        // TODO: Add pagination
        return await connection
            .QueryAsync<Salary>(query,
                new { Employee = employee, From = from, To = to });
    }
}