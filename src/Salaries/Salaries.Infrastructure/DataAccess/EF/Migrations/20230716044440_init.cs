using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Salaries.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "salaries");

            migrationBuilder.CreateTable(
                name: "Salaries",
                schema: "salaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "numeric", nullable: false),
                    Allowance = table.Column<decimal>(type: "numeric", nullable: false),
                    Transportation = table.Column<decimal>(type: "numeric", nullable: false),
                    OvertimeSalary = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalSalary = table.Column<decimal>(type: "numeric", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Employee = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_Employee",
                schema: "salaries",
                table: "Salaries",
                column: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_Employee_SalaryDate",
                schema: "salaries",
                table: "Salaries",
                columns: new[] { "Employee", "SalaryDate" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Salaries",
                schema: "salaries");
        }
    }
}
