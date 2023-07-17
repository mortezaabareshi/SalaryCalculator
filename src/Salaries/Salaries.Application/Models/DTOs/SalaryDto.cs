namespace Salaries.Application.Models.DTOs;

public class SalaryDto
{
    public Guid SalaryId { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowance { get; set; }
    public decimal Transportation { get; set; }
    public decimal OvertimeSalary { get; set; }
    public decimal TotalSalary { get; set; }
    public DateTime SalaryDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}