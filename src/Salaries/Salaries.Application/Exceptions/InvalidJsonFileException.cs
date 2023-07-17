using Salaries.Core.Exceptions;

namespace Salaries.Application.Exceptions;

public class InvalidJsonFileException : DomainException
{
    public override string Code { get; } = "invalid_json_file";

    public InvalidJsonFileException() : base("Invalid Json file.")
    {
    }
}