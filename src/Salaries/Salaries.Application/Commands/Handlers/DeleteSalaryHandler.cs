using Salaries.Application.Exceptions;
using Salaries.Core.Repositories;
using Shared.Infrastructure.Commands;

namespace Salaries.Application.Commands.Handlers;

public class DeleteSalaryHandler : ICommandHandler<DeleteSalary>
{
    private readonly ISalaryWriteRepository _salaryRepository;

    public DeleteSalaryHandler(ISalaryWriteRepository salaryRepository)
    {
        _salaryRepository = salaryRepository;
    }

    public async Task HandleAsync(DeleteSalary command, CancellationToken cancellationToken = default)
    {
        var salary =
            await _salaryRepository.GetAsync(command.SalaryId);

        if (salary is null)
        {
            throw new NotFoundSalary();
        }
        
        salary.Delete();

        await _salaryRepository.UpdateAsync(salary);
    }
}