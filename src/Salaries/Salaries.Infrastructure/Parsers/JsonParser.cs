using Newtonsoft.Json;
using Salaries.Application.Services;

namespace Salaries.Infrastructure.Parsers;

public class JsonParser<T> : IJsonParser<T> where T : class
{
    public async Task<IEnumerable<T>?> ParseFromStream(Stream stream)
    {
        using var reader = new StreamReader(stream);
        await using var jsonReader = new JsonTextReader(reader);
        var serializer = new JsonSerializer();
        return serializer.Deserialize<List<T>>(jsonReader);
    }
}