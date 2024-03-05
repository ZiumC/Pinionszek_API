using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupDetailedCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DetailedCategories",
                columns: new[] { "IdDetailedCategory", "IdGeneralCategory", "IdUser", "Name" },
                values: new object[,]
                {
                    { 1, 1, 1, "Rents" },
                    { 2, 1, 2, "Bills" },
                    { 3, 1, 1, "Health" },
                    { 4, 1, 1, "Fixed fee" },
                    { 5, 2, 1, "Pets" },
                    { 6, 2, 2, "Digital tools" },
                    { 7, 2, 3, "Phisical tools" },
                    { 8, 2, 1, "Food" },
                    { 9, 3, 1, "Future" },
                    { 10, 3, 2, "Future" },
                    { 11, 3, 3, "Future" },
                    { 12, 2, 2, "Food" },
                    { 13, 2, 3, "Food" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "DetailedCategories",
                keyColumn: "IdDetailedCategory",
                keyValue: 13);
        }
    }
}
