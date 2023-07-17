using Salaries.Application.Exceptions;
using Salaries.Application.Models.DTOs;
using Salaries.Core.Repositories;
using Salaries.Core.ValueObjects;
using Shared.Infrastructure.Queries;

namespace Salaries.Application.Queries.Handlers;

public class GetSalaryRangeHandler : IQueryHandler<GetSalaryRange, IEnumerable<SalaryDto>>
{
    private readonly ISalaryReadRepository _salaryRepository;

    public GetSalaryRangeHandler(ISalaryReadRepository salaryRepository)
    {
        _salaryRepository = salaryRepository;
    }

    public async Task<IEnumerable<SalaryDto>> HandleAsync(GetSalaryRange query, CancellationToken cancellationToken = default)
    {
        var salaries = await _salaryRepository.GetRangeAsync(new Employee(query.FirstName, query.LastName),
            query.FromDate, query.ToDate);
        
        if (salaries is null)
        {
            throw new NotFoundSalary();
        }

        return salaries.AsDtos();
    }
}