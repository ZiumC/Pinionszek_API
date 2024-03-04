using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class FixedReferencesBetweenBudgetBudgetStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Budgets_IdBudgetStatus",
                table: "Budgets");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_IdBudgetStatus",
                table: "Budgets",
                column: "IdBudgetStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Budgets_IdBudgetStatus",
                table: "Budgets");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_IdBudgetStatus",
                table: "Budgets",
                column: "IdBudgetStatus",
                unique: true);
        }
    }
}
