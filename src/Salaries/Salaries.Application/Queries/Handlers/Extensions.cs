using Salaries.Application.Models.DTOs;
using Salaries.Core.Entities;

namespace Salaries.Application.Queries.Handlers;

public static class Extensions
{
    public static SalaryDto AsDto(this Salary salary)
        => salary.Map<SalaryDto>();

    public static IEnumerable<SalaryDto> AsDtos(this IEnumerable<Salary> salaries)
        => salaries.Select(salary => salary.AsDto());
    
    private static T Map<T>(this Salary salary) where T : SalaryDto, new()
        => new()
        {
            SalaryId = salary.Id,
            BasicSalary = salary.BasicSalary,
            Allowance = salary.Allowance,
            Transportation = salary.Transportation,
            FirstName = salary.Employee.FirstName,
            LastName = salary.Employee.LastName,
            OvertimeSalary = salary.OvertimeSalary,
            SalaryDate = salary.SalaryDate,
            TotalSalary = salary.TotalSalary
        };
}