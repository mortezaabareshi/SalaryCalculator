using Microsoft.AspNetCore.Http;
using Salaries.Application.Models;
using Shared.Infrastructure.Commands;

namespace Salaries.Application.Commands;

public class AddSalaries : ICommand
{
    public PolicyOvertimeMethod PolicyOvertimeMethod { get; set; }
    public IFormFile Data { get; set; }
    public DataFormat Type { get; set; }
}