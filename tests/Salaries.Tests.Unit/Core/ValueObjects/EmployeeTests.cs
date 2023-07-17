using Salaries.Core.ValueObjects;
using Shouldly;

namespace Salaries.Tests.Unit.Core.ValueObjects;

public class EmployeeTests
{
    private static Employee Act(string value) => Employee.From(value);

    [Fact]
    public void given_valid_value_should_be_pass_validation()
    {
        //Arrange
        const string value = "David Low";
        
        //Act
        var employee = Act(value);
        
        //Assert
        employee.FirstName.ShouldBe("David");
        employee.LastName.ShouldBe("Low");
        employee.Equals(new Employee("David","Low")).ShouldBeTrue();
        employee.Equals(employee).ShouldBeTrue();
        employee.Equals(null).ShouldBeFalse();
        employee.Equals((object)null).ShouldBeFalse();
        employee.Equals((object)employee).ShouldBeTrue();
        employee.ToString().ShouldBe(value);
        ((string)employee).ShouldBe(value);
        ((Employee)(null)).ShouldBe(null);
        ((Employee)(value)).ShouldBe(employee);
        employee.GetHashCode().ShouldBeOfType<int>();
    }
}