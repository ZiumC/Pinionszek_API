using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupBudgetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Budgets",
                columns: new[] { "IdBudget", "BudgetYear", "IdBudgetStatus", "IdUser", "IsCompleted", "OpendDate", "Revenue", "Surplus" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, false, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2213m, 12m },
                    { 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 3, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 4, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 5, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 6, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 7, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 8, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 9, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 10, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 11, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 12, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, false, null, 0m, 0m },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, false, new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 3250m, 0m },
                    { 14, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 15, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 16, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 17, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 18, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 19, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 20, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 21, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 22, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 23, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 24, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, false, null, 0m, 0m },
                    { 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, false, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 120m },
                    { 26, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 27, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 28, new DateTime(2024, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 29, new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 30, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 31, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 32, new DateTime(2024, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 33, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 34, new DateTime(2024, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 35, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m },
                    { 36, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, false, null, 0m, 0m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Budgets",
                keyColumn: "IdBudget",
                keyValue: 36);
        }
    }
}
