using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class AddedReferencesBetweenBudgetPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdBudget",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IdBudget",
                table: "Payments",
                column: "IdBudget");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Budgets_IdBudget",
                table: "Payments",
                column: "IdBudget",
                principalTable: "Budgets",
                principalColumn: "IdBudget",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Budgets_IdBudget",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_IdBudget",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IdBudget",
                table: "Payments");
        }
    }
}
