using Salaries.Application.Models.DTOs;
using Shared.Infrastructure.Queries;

namespace Salaries.Application.Queries;

public class GetSalaryRange : IQuery<IEnumerable<SalaryDto>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}