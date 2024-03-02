using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class CreatedDetailedCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralCategory",
                table: "GeneralCategory");

            migrationBuilder.RenameTable(
                name: "GeneralCategory",
                newName: "GeneralCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralCategories",
                table: "GeneralCategories",
                column: "IdGeneralCategory");

            migrationBuilder.CreateTable(
                name: "DetailedCategories",
                columns: table => new
                {
                    IdDetailedCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    IdGeneralCategory = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailedCategories", x => x.IdDetailedCategory);
                    table.ForeignKey(
                        name: "FK_DetailedCategories_GeneralCategories_IdGeneralCategory",
                        column: x => x.IdGeneralCategory,
                        principalTable: "GeneralCategories",
                        principalColumn: "IdGeneralCategory",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailedCategories_IdGeneralCategory",
                table: "DetailedCategories",
                column: "IdGeneralCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailedCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneralCategories",
                table: "GeneralCategories");

            migrationBuilder.RenameTable(
                name: "GeneralCategories",
                newName: "GeneralCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneralCategory",
                table: "GeneralCategory",
                column: "IdGeneralCategory");
        }
    }
}
