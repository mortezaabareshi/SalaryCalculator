using Salaries.Core.Entities;
using Salaries.Core.ValueObjects;

namespace Salaries.Core.Repositories;

public interface ISalaryReadRepository
{
    Task<Salary?> GetAsync(Employee employee, DateTime salaryDate);
    Task<IEnumerable<Salary>> GetRangeAsync(Employee employee, DateTime from, DateTime to);
}