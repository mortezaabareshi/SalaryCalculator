using Salaries.Core.Exceptions;

namespace Salaries.Application.Exceptions;

public class InvalidCustomFileException : DomainException
{
    public override string Code { get; } = "invalid_custom_file";

    public InvalidCustomFileException() : base("Invalid custom file.")
    {
    }
}