using Salaries.Application.Models.DTOs;
using Shared.Infrastructure.Queries;

namespace Salaries.Application.Queries;

public class GetSalary : IQuery<SalaryDto>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime SalaryDate { get; set; }
}