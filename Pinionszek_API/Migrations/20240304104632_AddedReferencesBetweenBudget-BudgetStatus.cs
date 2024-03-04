using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class AddedReferencesBetweenBudgetBudgetStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdBudgetStatus",
                table: "Budget",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Budget_IdBudgetStatus",
                table: "Budget",
                column: "IdBudgetStatus",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Budget_BudgetStatuses_IdBudgetStatus",
                table: "Budget",
                column: "IdBudgetStatus",
                principalTable: "BudgetStatuses",
                principalColumn: "IdBudgetStatus",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budget_BudgetStatuses_IdBudgetStatus",
                table: "Budget");

            migrationBuilder.DropIndex(
                name: "IX_Budget_IdBudgetStatus",
                table: "Budget");

            migrationBuilder.DropColumn(
                name: "IdBudgetStatus",
                table: "Budget");
        }
    }
}
