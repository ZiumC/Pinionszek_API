using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class AddedReferencesBetweenUserDetailedCat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUser",
                table: "DetailedCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DetailedCategories_IdUser",
                table: "DetailedCategories",
                column: "IdUser");

            migrationBuilder.AddForeignKey(
                name: "FK_DetailedCategories_Users_IdUser",
                table: "DetailedCategories",
                column: "IdUser",
                principalTable: "Users",
                principalColumn: "IdUser",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetailedCategories_Users_IdUser",
                table: "DetailedCategories");

            migrationBuilder.DropIndex(
                name: "IX_DetailedCategories_IdUser",
                table: "DetailedCategories");

            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "DetailedCategories");
        }
    }
}
