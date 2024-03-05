using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "IdPayment", "Charge", "IdBudget", "IdDetailedCategory", "IdPaymentStatus", "Message", "Name", "PaidOn", "PaymentAddedOn", "PaymentDate", "Refund" },
                values: new object[,]
                {
                    { 1, 180m, 1, 5, 2, "Fryzjer małego pieska domowego", "Fryzjer dla psa", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m },
                    { 2, 670m, 1, 1, 2, "", "Mieszkanie", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m },
                    { 3, 570m, 1, 3, 1, "", "Prywatne ubezpieczenie zdrowotne", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m },
                    { 4, 35.99m, 1, 4, 2, "", "Platforma Netflix", null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 11m },
                    { 5, 11.23m, 1, 8, 1, "", "Kaufland", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m },
                    { 6, 100m, 1, 9, 1, "", "Savings", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m },
                    { 7, 861m, 13, 2, 2, "", "Czynsz", null, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m },
                    { 8, 425m, 13, 6, 1, "", "Oprogramowanie do montowania dysków", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m },
                    { 9, 200m, 13, 10, 1, "", "Savings", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m },
                    { 10, 99.12m, 13, 12, 1, "", "Lidl", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 40m },
                    { 11, 7.89m, 13, 12, 1, "Podroby dla piesków", "Biedronka", new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m },
                    { 12, 53.12m, 1, 8, 1, "", "Biedronka", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0m },
                    { 13, 305m, 13, 2, 2, "", "Opieka zdrowotna", null, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "IdPayment",
                keyValue: 13);
        }
    }
}
