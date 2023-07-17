using Shared.Infrastructure.Commands;

namespace Salaries.Application.Commands;

public class DeleteSalary : ICommand
{
    public Guid SalaryId { get; set; }
}