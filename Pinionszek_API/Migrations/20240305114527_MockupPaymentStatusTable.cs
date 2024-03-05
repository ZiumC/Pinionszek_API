using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupPaymentStatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "IdPaymentStatus", "Name" },
                values: new object[] { 1, "PAYED" });

            migrationBuilder.InsertData(
                table: "PaymentStatuses",
                columns: new[] { "IdPaymentStatus", "Name" },
                values: new object[] { 2, "NOT PAYED YET" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "IdPaymentStatus",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentStatuses",
                keyColumn: "IdPaymentStatus",
                keyValue: 2);
        }
    }
}
