using Microsoft.EntityFrameworkCore;
using Salaries.Core.Entities;
using Salaries.Core.Repositories;

namespace Salaries.Infrastructure.DataAccess.EF.Repositories;

public class SalaryEFRepository : ISalaryWriteRepository
{
    private readonly EFDbContext _efDbContext;
    private readonly DbSet<Salary> _salaries;

    public SalaryEFRepository(EFDbContext efDbContext)
    {
        _efDbContext = efDbContext;
        _salaries = _efDbContext.Salaries;
    }

    public async Task AddAsync(Salary salary)
    {
        await _salaries.AddAsync(salary);
        await _efDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Salary salary)
    {
        _salaries.Update(salary);
        await _efDbContext.SaveChangesAsync();
    }

    public async Task<Salary?> GetAsync(Guid id)
    {
        return await _salaries.SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }
    
}