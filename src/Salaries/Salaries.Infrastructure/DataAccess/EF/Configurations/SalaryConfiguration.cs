using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Salaries.Core.Entities;
using Salaries.Core.ValueObjects;

namespace Salaries.Infrastructure.DataAccess.EF.Configurations;

public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.Property(x => x.Employee).IsRequired().HasMaxLength(500)
            .HasConversion(x => x.ToString(), x => Employee.From(x));

        builder.HasIndex(x => x.Employee);
        builder.HasIndex(x =>
        new {
            x.Employee, x.SalaryDate
        }).IsUnique();
    }
}