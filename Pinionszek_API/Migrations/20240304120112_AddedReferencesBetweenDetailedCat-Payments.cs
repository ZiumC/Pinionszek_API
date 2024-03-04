using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class AddedReferencesBetweenDetailedCatPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DetailedCategoryIdDetailedCategory",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdDetailedCategory",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DetailedCategoryIdDetailedCategory",
                table: "Payments",
                column: "DetailedCategoryIdDetailedCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_DetailedCategories_DetailedCategoryIdDetailedCategory",
                table: "Payments",
                column: "DetailedCategoryIdDetailedCategory",
                principalTable: "DetailedCategories",
                principalColumn: "IdDetailedCategory",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_DetailedCategories_DetailedCategoryIdDetailedCategory",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DetailedCategoryIdDetailedCategory",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DetailedCategoryIdDetailedCategory",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IdDetailedCategory",
                table: "Payments");
        }
    }
}
