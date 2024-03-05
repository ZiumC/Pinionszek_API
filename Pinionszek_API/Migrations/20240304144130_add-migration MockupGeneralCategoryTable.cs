using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class addmigrationMockupGeneralCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GeneralCategories",
                columns: new[] { "IdGeneralCategory", "IsDefault", "Name" },
                values: new object[] { 1, true, "Needs" });

            migrationBuilder.InsertData(
                table: "GeneralCategories",
                columns: new[] { "IdGeneralCategory", "IsDefault", "Name" },
                values: new object[] { 2, true, "Wants" });

            migrationBuilder.InsertData(
                table: "GeneralCategories",
                columns: new[] { "IdGeneralCategory", "IsDefault", "Name" },
                values: new object[] { 3, true, "Savings" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GeneralCategories",
                keyColumn: "IdGeneralCategory",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GeneralCategories",
                keyColumn: "IdGeneralCategory",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GeneralCategories",
                keyColumn: "IdGeneralCategory",
                keyValue: 3);
        }
    }
}
