using Salaries.Application.Exceptions;
using Salaries.Application.Models.DTOs;
using Salaries.Core.Repositories;
using Salaries.Core.ValueObjects;
using Shared.Infrastructure.Queries;

namespace Salaries.Application.Queries.Handlers;

public class GetSalaryHandler : IQueryHandler<GetSalary, SalaryDto>
{
    private readonly ISalaryReadRepository _salaryRepository;

    public GetSalaryHandler(ISalaryReadRepository salaryRepository)
    {
        _salaryRepository = salaryRepository;
    }

    public async Task<SalaryDto> HandleAsync(GetSalary query, CancellationToken cancellationToken = default)
    {
        var salary = await _salaryRepository.GetAsync(new Employee(query.FirstName, query.LastName), query.SalaryDate);
        
        if (salary is null)
        {
            throw new NotFoundSalary();
        }

        return salary.AsDto();
    }
}