using System.Data;
using Dapper;
using Salaries.Core.ValueObjects;

namespace Salaries.Infrastructure.DataAccess.Dapper.Conversions;

public class EmployeeConversion : SqlMapper.TypeHandler<Employee>
{
    public override void SetValue(IDbDataParameter parameter, Employee value)
    {
        parameter.Value = value.ToString();
    }

    public override Employee Parse(object value)
    {
        return Employee.From(value.ToString()!);
    }
}