namespace Salaries.Application.Services;

public interface IJsonParser<T> where T : class
{
     Task<IEnumerable<T>?> ParseFromStream(Stream stream);
}