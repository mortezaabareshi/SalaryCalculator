using Microsoft.AspNetCore.Http;
using OvertimePolicies;
using Salaries.Application.Exceptions;
using Salaries.Application.Models;
using Salaries.Application.Models.DTOs;
using Salaries.Application.Services;
using Salaries.Core.Entities;
using Salaries.Core.Repositories;
using Salaries.Core.ValueObjects;
using Shared.Infrastructure.Commands;

namespace Salaries.Application.Commands.Handlers;

public sealed class AddSalariesHandler : ICommandHandler<AddSalaries>
{
    private readonly IJsonParser<SalaryDto> _jsonParser;
    private readonly IXmlParser _xmlParser;
    private readonly ISalaryWriteRepository _salaryRepository;
    private readonly IOvertimeCalculator _overtimeCalculator;
    
    
    public AddSalariesHandler(IXmlParser xmlParser, ISalaryWriteRepository salaryRepository,
        IOvertimeCalculator overtimeCalculator, IJsonParser<SalaryDto> jsonParser)
    {
        _xmlParser = xmlParser;
        _salaryRepository = salaryRepository;
        _overtimeCalculator = overtimeCalculator;
        _jsonParser = jsonParser;
    }
    public async Task HandleAsync(AddSalaries command, CancellationToken cancellationToken = default)
    {
        var overtimeSalary = command.PolicyOvertimeMethod switch
        {
            PolicyOvertimeMethod.CalculatorA => _overtimeCalculator.CalculatorA(),
            PolicyOvertimeMethod.CalculatorB => _overtimeCalculator.CalculatorB(),
            PolicyOvertimeMethod.CalculatorC => _overtimeCalculator.CalculatorC(),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        var salaries = command.Type switch
        {
            DataFormat.Xml => await GetSalariesFromXml(command.Data, overtimeSalary),
            DataFormat.Json => await GetSalariesFromJson(command.Data, overtimeSalary),
            DataFormat.Custom => await GetSalariesFromCustom(command.Data, overtimeSalary),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        //bottleneck
        foreach (var salary in salaries)
        {
            await _salaryRepository.AddAsync(salary);
        }
    }

    private async Task<IEnumerable<Salary>> GetSalariesFromXml(IFormFile data, decimal overtimeSalary)
    {
        await using var inputFileStream = data.OpenReadStream();
        var cells = _xmlParser.GetSheetCells(inputFileStream, "salaries");
        var countRows = _xmlParser.GetSheetRowCount(inputFileStream, "salaries");
        var basicSalaryCN = cells
            .SingleOrDefault(x => x.Key.Row == 1 && x.Value?.Trim() == nameof(Salary.BasicSalary)).Key.Column;
        var allowanceCN = cells
            .SingleOrDefault(x => x.Key.Row == 1 && x.Value?.Trim() == nameof(Salary.Allowance)).Key.Column;
        var transportationCN = cells
            .SingleOrDefault(x => x.Key.Row == 1 && x.Value?.Trim() == nameof(Salary.Transportation)).Key.Column;
        var salaryDateCN = cells
            .SingleOrDefault(x => x.Key.Row == 1 && x.Value?.Trim() == nameof(Salary.SalaryDate)).Key.Column;
        var firstNameCN = cells
            .SingleOrDefault(x => x.Key.Row == 1 && x.Value?.Trim() == nameof(Employee.FirstName)).Key.Column;
        var lastNameCN = cells
            .SingleOrDefault(x => x.Key.Row == 1 && x.Value?.Trim() == nameof(Employee.LastName)).Key.Column;


        var salaries = new List<Salary>();
        for (var rowIndex = 2; rowIndex <= countRows; rowIndex++)
        {
            try
            {
                var basicSalary = decimal.Parse(cells
                    .FirstOrDefault(x => x.Key.Row == rowIndex && x.Key.Column == basicSalaryCN).Value);
                var allowance = decimal.Parse(cells
                    .FirstOrDefault(x => x.Key.Row == rowIndex && x.Key.Column == allowanceCN).Value);
                var transportation = decimal.Parse(cells
                    .FirstOrDefault(x => x.Key.Row == rowIndex && x.Key.Column == transportationCN).Value);
                DateTime salaryDate;
                DateTime.TryParseExact(cells
                    .FirstOrDefault(x => x.Key.Row == rowIndex && x.Key.Column == salaryDateCN).Value,
                    "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out salaryDate);
                var firstName = cells
                    .FirstOrDefault(x => x.Key.Row == rowIndex && x.Key.Column == firstNameCN).Value;
                var lastName = cells
                    .FirstOrDefault(x => x.Key.Row == rowIndex && x.Key.Column == lastNameCN).Value;
                
                salaries.Add(new Salary(basicSalary, allowance, transportation, overtimeSalary, salaryDate,
                    new Employee(firstName, lastName), DateTime.Now));
            }
            catch (Exception e)
            {
                throw new InvalidXmlFileException();
            }
        }

        return salaries;
    }
    
    private async Task<IEnumerable<Salary>> GetSalariesFromJson(IFormFile data, decimal overtimeSalary)
    {
        await using var inputFileStream = data.OpenReadStream();

        var salariesDto = await _jsonParser.ParseFromStream(inputFileStream);
        if (salariesDto is null)
        {
            throw new InvalidJsonFileException();
        }

        var salaries = salariesDto
            .Select(salaryDto => new Salary(salaryDto.BasicSalary, salaryDto.Allowance, salaryDto.Transportation, 
                overtimeSalary, salaryDto.SalaryDate, new Employee(salaryDto.FirstName, salaryDto.LastName), DateTime.Now));

        return salaries;
    }

    private async Task<IEnumerable<Salary>> GetSalariesFromCustom(IFormFile data, decimal overtimeSalary)
    {
        await using var inputFileStream = data.OpenReadStream();
        using var streamReader = new StreamReader(inputFileStream);
        var firstLine = await streamReader.ReadLineAsync();
        if (firstLine == null)
        {
            throw new InvalidCustomFileException();
        }

        var columns = firstLine.Split("/");
        if (columns.Length != 6)
        {
            throw new InvalidCustomFileException();
        }

        var basicSalaryCN = Array.FindIndex(columns, s => s.Trim() == nameof(Salary.BasicSalary));
        ;
        var allowanceCN = Array.FindIndex(columns, s => s.Trim() == nameof(Salary.Allowance));
        ;
        var transportationCN = Array.FindIndex(columns, s => s.Trim() == nameof(Salary.Transportation));
        ;
        var salaryDateCN = Array.FindIndex(columns, s => s.Trim() == nameof(Salary.SalaryDate));
        ;
        var firstNameCN = Array.FindIndex(columns, s => s.Trim() == nameof(Employee.FirstName));
        ;
        var lastNameCN = Array.FindIndex(columns, s => s.Trim() == nameof(Employee.LastName));
        ;

        var salaries = new List<Salary>();
        while (!streamReader.EndOfStream)
        {
            var line = await streamReader.ReadLineAsync();

            try
            {
                var cells = line.Split("/");
                var basicSalary = decimal.Parse(cells[basicSalaryCN].Trim());
                var allowance = decimal.Parse(cells[allowanceCN].Trim());
                var transportation = decimal.Parse(cells[transportationCN].Trim());
                DateTime salaryDate;
                DateTime.TryParseExact(cells[salaryDateCN], "yyyyMMdd", null,
                    System.Globalization.DateTimeStyles.None, out salaryDate);
                var firstName = cells[firstNameCN].Trim();
                var lastName = cells[lastNameCN].Trim();

                salaries.Add(new Salary(basicSalary, allowance, transportation, overtimeSalary, salaryDate,
                    new Employee(firstName, lastName), DateTime.Now));
            }
            catch (Exception e)
            {
                throw new InvalidCustomFileException();
            }
        }

        return salaries;
    }
}