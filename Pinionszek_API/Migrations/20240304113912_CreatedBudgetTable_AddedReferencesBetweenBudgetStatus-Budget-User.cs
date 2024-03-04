using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class CreatedBudgetTable_AddedReferencesBetweenBudgetStatusBudgetUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    IdBudget = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    OpendDate = table.Column<DateTime>(type: "date", nullable: true),
                    Revenue = table.Column<decimal>(type: "money", nullable: false),
                    Surplus = table.Column<decimal>(type: "money", nullable: false),
                    BudgetYear = table.Column<DateTime>(type: "date", nullable: false),
                    IdBudgetStatus = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.IdBudget);
                    table.ForeignKey(
                        name: "FK_Budgets_BudgetStatuses_IdBudgetStatus",
                        column: x => x.IdBudgetStatus,
                        principalTable: "BudgetStatuses",
                        principalColumn: "IdBudgetStatus",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Budgets_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_IdBudgetStatus",
                table: "Budgets",
                column: "IdBudgetStatus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_IdUser",
                table: "Budgets",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Budgets");
        }
    }
}
