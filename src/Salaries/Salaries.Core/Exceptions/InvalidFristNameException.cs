namespace Salaries.Core.Exceptions;

public class InvalidFirstNameException : DomainException
{
    public override string Code { get; } = "invalid_first_name";
    public string FirstName { get; }

    public InvalidFirstNameException(string firstName) : base($"Invalid first name : {firstName}")
    {
        FirstName = firstName;
    }
}