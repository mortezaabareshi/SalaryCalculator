using Microsoft.AspNetCore.Http;
using NSubstitute;
using OvertimePolicies;
using Salaries.Application.Commands;
using Salaries.Application.Commands.Handlers;
using Salaries.Application.Models;
using Salaries.Application.Models.DTOs;
using Salaries.Application.Services;
using Salaries.Application.Services.Models;
using Salaries.Core.Entities;
using Salaries.Core.Repositories;
using Salaries.Core.ValueObjects;

namespace Salaries.Tests.Unit.Application.Commands;

public class AddSalariesHandlerTests
{
    private Task Act(AddSalaries command) => _handler.HandleAsync(command);

    [Fact]
    public async Task should_parse_salary_from_xml()
    {
        //Arrange
        var stream = Stream.Null;
        IFormFile data = new FormFile(stream, 0, stream.Length, null, "");
        var command = new AddSalaries
        {
            Data = data,
            Type = DataFormat.Xml,
            PolicyOvertimeMethod = PolicyOvertimeMethod.CalculatorA
        };
        var salaryDate = DateTime.Parse("07/01/2023");
        var receivedSalary = new Salary(5000, 200, 100, 200, salaryDate,
            new Employee("Johssn", "Doe"), DateTime.Now);

        _xmlParser.GetSheetCells(Arg.Any<Stream>(), Arg.Any<string>()).Returns(_cells);
        _xmlParser.GetSheetRowCount(Arg.Any<Stream>(), Arg.Any<string>()).Returns(2);

        _overtimeCalculator.CalculatorA().Returns(200);
        
        //Act
        await Act(command);

        //Assert
        await _salaryRepository.Received().AddAsync(Arg.Is<Salary>(x => x.IsDeleted == false
            && x.BasicSalary == receivedSalary.BasicSalary 
            && x.Allowance == receivedSalary.Allowance
            && x.Employee.Equals(receivedSalary.Employee)
            && x.Transportation == receivedSalary.Transportation
            && x.SalaryDate.Date.Equals(receivedSalary.SalaryDate.Date)));
    }

    [Fact]
    public async Task should_parse_salary_from_json()
    {
        //Arrange
        var stream = Stream.Null;
        IFormFile data = new FormFile(stream, 0, stream.Length, null, "");
        var command = new AddSalaries
        {
            Data = data,
            Type = DataFormat.Json,
            PolicyOvertimeMethod = PolicyOvertimeMethod.CalculatorA
        };
        _overtimeCalculator.CalculatorA().Returns(200);
        var salaryDate = DateTime.Parse("07/01/2023");
        var salaryDto = new SalaryDto
        {
            Allowance = 200,
            Transportation = 100,
            BasicSalary = 5000,
            FirstName = "Johssn",
            LastName = "Doe",
            SalaryDate = salaryDate
        };
        _jsonParser.ParseFromStream(Arg.Any<Stream>()).Returns(new List<SalaryDto>{ salaryDto });

        //Act
        await Act(command);

        //Assert
        await _salaryRepository.Received().AddAsync(Arg.Is<Salary>(x => x.IsDeleted == false
            && x.BasicSalary == salaryDto.BasicSalary 
            && x.Allowance == salaryDto.Allowance
            && x.Employee.Equals(new Employee(salaryDto.FirstName, salaryDto.LastName))
            && x.Transportation == salaryDto.Transportation
            && x.SalaryDate.Date.Equals(salaryDto.SalaryDate.Date)));
    }

    [Fact]
    public async Task should_parse_salary_from_custom()
    {
        //Arrange
        var content = "FirstName/LastName/BasicSalary/Allowance/Transportation/SalaryDate\n" +
                      "Johssn/Doe/5000/200/100/20230701";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        await writer.WriteAsync(content);
        await writer.FlushAsync();
        stream.Position = 0;
        IFormFile data = new FormFile(stream, 0, stream.Length, "form", "test.txt");
        var command = new AddSalaries
        {
            Data = data,
            Type = DataFormat.Custom,
            PolicyOvertimeMethod = PolicyOvertimeMethod.CalculatorA
        };
        var salaryDate = DateTime.Parse("07/01/2023");
        var salaryDto = new SalaryDto
        {
            Allowance = 200,
            Transportation = 100,
            BasicSalary = 5000,
            FirstName = "Johssn",
            LastName = "Doe",
            SalaryDate = salaryDate
        };
        
        //Act
        await Act(command);
        
        //Assert
        await _salaryRepository.Received().AddAsync(Arg.Is<Salary>(x => x.IsDeleted == false
            && x.BasicSalary == salaryDto.BasicSalary 
            && x.Allowance == salaryDto.Allowance
            && x.Employee.Equals(new Employee(salaryDto.FirstName, salaryDto.LastName))
            && x.Transportation == salaryDto.Transportation
            && x.SalaryDate.Date.Equals(salaryDto.SalaryDate.Date)));
    }
    
    #region Arrange

    private readonly AddSalariesHandler _handler;
    private readonly IXmlParser _xmlParser;
    private readonly IJsonParser<SalaryDto> _jsonParser;
    private readonly ISalaryWriteRepository _salaryRepository;
    private readonly IOvertimeCalculator _overtimeCalculator;

    public AddSalariesHandlerTests()
    {
        _salaryRepository = Substitute.For<ISalaryWriteRepository>();
        _xmlParser = Substitute.For<IXmlParser>();
        _jsonParser = Substitute.For<IJsonParser<SalaryDto>>();
        _overtimeCalculator = Substitute.For<IOvertimeCalculator>();
        _handler = new AddSalariesHandler(_xmlParser, _salaryRepository, _overtimeCalculator, _jsonParser);
    }
    
    private readonly Dictionary<SheetCell, string> _cells = new Dictionary<SheetCell, string>
        {
            {
                new SheetCell
                {
                    Column = 1,
                    Row = 1
                },
                "FirstName"
            },
            {
                new SheetCell
                {
                    Column = 2,
                    Row = 1
                },
                "LastName"
            },
            {
                new SheetCell
                {
                    Column = 3,
                    Row = 1
                },
                "BasicSalary"
            },
            {
                new SheetCell
                {
                    Column = 4,
                    Row = 1
                },
                "Allowance"
            },
            {
                new SheetCell
                {
                    Column = 5,
                    Row = 1
                },
                "Transportation"
            },
            {
                new SheetCell
                {
                    Column = 6,
                    Row = 1
                },
                "SalaryDate"
            },
            {
                new SheetCell
                {
                    Column = 1,
                    Row = 2
                },
                "Johssn"
            },
            {
                new SheetCell
                {
                    Column = 2,
                    Row = 2
                },
                "Doe"
            },
            { new SheetCell
            {
                Column = 3,
                Row = 2
            }, "5000" },
            { new SheetCell
            {
                Column = 4,
                Row = 2
            }, "200" },
            { new SheetCell
            {
                Column = 5,
                Row = 2
            }, "100" },
            { new SheetCell
            {
                Column = 6,
                Row = 2
            }, "20230701" }
        };

    #endregion
}