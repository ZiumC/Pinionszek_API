using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class CreatedBudgetStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetStatuses",
                columns: table => new
                {
                    IdBudgetStatus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetStatuses", x => x.IdBudgetStatus);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetStatuses");
        }
    }
}
