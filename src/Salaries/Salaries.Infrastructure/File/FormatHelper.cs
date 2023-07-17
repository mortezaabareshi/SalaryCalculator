namespace Salaries.Infrastructure.File;

public class FormatHelper
{
    public bool IsXmlData(byte[] file)
    {
        if (file.Length < 2)
            return false;

        var buffer = new char[5];
        using var stream = new MemoryStream(file);
        new StreamReader(stream).Read(buffer, 0, 5);
        var signature = string.Concat(buffer);
        return signature == "<?xml";
    }
}