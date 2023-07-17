using Salaries.Core.Exceptions;

namespace Salaries.Application.Exceptions;

public class InvalidXmlFileException : DomainException
{
    public override string Code { get; } = "invalid_xml_file";

    public InvalidXmlFileException() : base("Invalid Xml file.")
    {
    }
}