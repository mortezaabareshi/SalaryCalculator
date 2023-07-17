using Salaries.Core.Entities;
using Salaries.Core.ValueObjects;
using Shouldly;

namespace Salaries.Tests.Unit.Core.Entities;

public class SalaryTests
{
    private static Salary Act(decimal basicSalary, decimal allowance, decimal transportation, decimal overtimeSalary,
        DateTime salaryDate, Employee employee, DateTime createdAt)
    {
        return new Salary(basicSalary, allowance, transportation, overtimeSalary, salaryDate, employee, createdAt);
    }

    [Fact]
    public void given_valid_argument_and_calculate_total_salaries_should_be_correct_calculator()
    {
        //Arrange
        decimal basicSalary = 1000;
        decimal allowance = 50;
        decimal transportation = 200;
        decimal overtimeSalary = 400;
        var salaryDate = DateTime.Now;
        var employee = new Employee("David", "Low");
        var createdAt = DateTime.Now;
        
        //Act
        var salary = Act(basicSalary, allowance, transportation, overtimeSalary, salaryDate, employee, createdAt);
        
        //Assert
        salary.ShouldNotBeNull();
        salary.TotalSalary.ShouldBe(1485);
    }
}