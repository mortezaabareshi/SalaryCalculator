using Salaries.Core.ValueObjects;

namespace Salaries.Core.Entities;

public class Salary : AggregateRoot
{
    public decimal BasicSalary { get; private set; }
    public decimal Allowance { get; private set; }
    public decimal Transportation { get; private set; }
    public decimal OvertimeSalary { get; private set; }
    public decimal TotalSalary { get; private set; }
    public DateTime SalaryDate { get; private set; }
    public Employee Employee { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public Salary()
    {
    }
    
    public Salary(decimal basicSalary, decimal allowance, decimal transportation, decimal overtimeSalary,
        DateTime salaryDate, Employee employee, DateTime createdAt)
    {
        Id = Guid.NewGuid();
        BasicSalary = basicSalary;
        Allowance = allowance;
        Transportation = transportation;
        OvertimeSalary = overtimeSalary;
        SalaryDate = salaryDate;
        Employee = employee;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        IsDeleted = false;
        TotalSalary = CalculateTotalSalary();
    }
    
    public void Update(decimal basicSalary, decimal allowance, decimal transportation, DateTime salaryDate,
        Employee employee, DateTime updatedAt)
    {
        BasicSalary = basicSalary;
        Allowance = allowance;
        Transportation = transportation;
        SalaryDate = salaryDate;
        Employee = employee;
        UpdatedAt = updatedAt;
        IsDeleted = false;
        TotalSalary = CalculateTotalSalary();
    }

    private decimal CalculateTotalSalary()
    {
        var totalSalary = BasicSalary + Allowance + Transportation + OvertimeSalary;
        totalSalary -= ((0.1m) * totalSalary);

        return totalSalary;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}