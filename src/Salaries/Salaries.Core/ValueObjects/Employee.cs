using Salaries.Core.Exceptions;

namespace Salaries.Core.ValueObjects;

public class Employee : IEquatable<Employee>
{
    public string FirstName { get; }
    public string LastName { get; }

    public Employee(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new InvalidFirstNameException(firstName);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new InvalidLastNameException(lastName);
        }
        
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public static implicit operator Employee(string value) => From(value);

    public static Employee From(string value)
    {
        var (firstName, lastName) = Split(value);

        return new Employee(firstName, lastName);
    }

    private static (string firstName, string lastName) Split(string value)
    {
        var values = value.Split(" ");
        return (values[0], values[1]);
    }

    public static implicit operator string(Employee value) => $"{value.FirstName} {value.LastName}";
    
    public bool Equals(Employee? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return FirstName == other.FirstName && LastName == other.LastName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((Employee)obj);
    }

    public override int GetHashCode() => HashCode.Combine(FirstName, LastName);

    public override string ToString() => $"{FirstName} {LastName}";
}