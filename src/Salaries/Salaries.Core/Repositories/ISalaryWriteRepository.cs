using Salaries.Core.Entities;

namespace Salaries.Core.Repositories;

public interface ISalaryWriteRepository
{
    Task<Salary?> GetAsync(Guid id);
    Task AddAsync(Salary salary);
    Task UpdateAsync(Salary salary);
}