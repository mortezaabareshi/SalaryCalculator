namespace Salaries.Infrastructure.Exceptions;

public static class Extensions
{
    public static string Underscore(this string value)
    {
       return string.Concat(value
               .Select<char, string>((Func<char, int, string>) ((x, i) => i <= 0 || !char.IsUpper(x) ? x.ToString() : "_" + x.ToString())))
           .ToLowerInvariant();
    }
}