using Microsoft.AspNetCore.Mvc;
using Salaries.Application.Commands;
using Salaries.Application.Models;
using Salaries.Application.Models.DTOs;
using Salaries.Application.Queries;
using Shared.Infrastructure.Dispatchers;

namespace Salaries.Api.Controllers;

[ApiController]
public class SalaryController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public SalaryController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    [HttpGet("/salaries/get-salary")]
    public async Task<ActionResult<SalaryDto>> GetSalary([FromQuery] GetSalary query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpGet("/salaries/get-salary-range")]
    public async Task<ActionResult<IEnumerable<SalaryDto>>> GetSalaries([FromQuery] GetSalaryRange query)
    {
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpPost("/salaries/{dataType}")]
    public async Task<ActionResult> AddSalary([FromRoute] DataFormat dataType,
        IFormFile data, [FromQuery] PolicyOvertimeMethod method)
    {
        var sth = dataType;
        await _dispatcher.SendAsync(new AddSalaries
        {
            Data = data,
            Type = dataType,
            PolicyOvertimeMethod = method
        });
        return NoContent();
    }

    [HttpPut("/salaries")]
    public async Task<ActionResult> UpdateSalary([FromBody] UpdateSalary command)
    {
        await _dispatcher.SendAsync(command);
        return NoContent();
    }

    [HttpDelete("/salaries")]
    public async Task<ActionResult> DeleteSalary([FromBody] DeleteSalary command)
    {
        await _dispatcher.SendAsync(command);
        return NoContent();
    }

}