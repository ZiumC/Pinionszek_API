using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupBudgetStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BudgetStatuses",
                columns: new[] { "IdBudgetStatus", "Name" },
                values: new object[] { 1, "OPEND" });

            migrationBuilder.InsertData(
                table: "BudgetStatuses",
                columns: new[] { "IdBudgetStatus", "Name" },
                values: new object[] { 2, "COMPLETED" });

            migrationBuilder.InsertData(
                table: "BudgetStatuses",
                columns: new[] { "IdBudgetStatus", "Name" },
                values: new object[] { 3, "NOT OPEND YET" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BudgetStatuses",
                keyColumn: "IdBudgetStatus",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BudgetStatuses",
                keyColumn: "IdBudgetStatus",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BudgetStatuses",
                keyColumn: "IdBudgetStatus",
                keyValue: 3);
        }
    }
}
