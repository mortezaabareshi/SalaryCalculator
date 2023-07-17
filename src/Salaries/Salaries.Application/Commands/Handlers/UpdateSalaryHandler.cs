using Salaries.Application.Exceptions;
using Salaries.Core.Repositories;
using Salaries.Core.ValueObjects;
using Shared.Infrastructure.Commands;

namespace Salaries.Application.Commands.Handlers;

public class UpdateSalaryHandler : ICommandHandler<UpdateSalary>
{
    private readonly ISalaryWriteRepository _salaryRepository;
    
    public UpdateSalaryHandler(ISalaryWriteRepository salaryRepository)
    {
        _salaryRepository = salaryRepository;
    }
    public async Task HandleAsync(UpdateSalary command, CancellationToken cancellationToken = default)
    {
        var salary =
            await _salaryRepository.GetAsync(command.SalaryId);
        
        if (salary is null)
        {
            throw new NotFoundSalary();
        }
        
        salary.Update(command.BasicSalary, command.Allowance, command.Transportation, command.SalaryDate,
            new Employee(command.FirstName, command.LastName), DateTime.Now);

        await _salaryRepository.UpdateAsync(salary);
    }
}