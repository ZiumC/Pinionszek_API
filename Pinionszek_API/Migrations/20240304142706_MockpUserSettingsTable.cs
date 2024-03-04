using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockpUserSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserSettings",
                columns: new[] { "IdUserSetting", "DisplayBudgetRules", "IdUser", "Needs", "Savings", "UseBudgetRules", "Wants" },
                values: new object[,]
                {
                    { 1, false, 2, 0m, 0m, false, 0m },
                    { 2, false, 1, 60m, 10m, true, 30m },
                    { 3, true, 4, 70m, 5m, true, 25m },
                    { 4, true, 3, 55m, 15m, true, 40m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "IdUserSetting",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "IdUserSetting",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "IdUserSetting",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "IdUserSetting",
                keyValue: 4);
        }
    }
}
