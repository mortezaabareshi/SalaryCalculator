using Salaries.Core.Exceptions;

namespace Salaries.Application.Exceptions;

public class NotFoundSalary : DomainException
{
    public override string Code { get; } = "not_found_salary";

    public NotFoundSalary() : base($"Not found salary.")
    {
    }
}