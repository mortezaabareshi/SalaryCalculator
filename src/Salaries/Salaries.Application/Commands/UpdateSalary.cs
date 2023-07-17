using Shared.Infrastructure.Commands;

namespace Salaries.Application.Commands;

public class UpdateSalary: ICommand
{
    public Guid SalaryId { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Transportation { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime SalaryDate { get; set; }
}