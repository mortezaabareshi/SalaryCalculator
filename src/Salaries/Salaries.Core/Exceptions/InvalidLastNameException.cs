namespace Salaries.Core.Exceptions;

public class InvalidLastNameException : DomainException
{
    public override string Code { get; } = "invalid_last_name";
    public string LastName { get; }

    public InvalidLastNameException(string lastName) : base($"Invalid first name : {lastName}")
    {
        LastName = lastName;
    }
}