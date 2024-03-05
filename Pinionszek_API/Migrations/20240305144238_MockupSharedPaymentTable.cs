using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupSharedPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SharedPayments",
                columns: new[] { "IdSharedPayment", "IdFriend", "IdPayment" },
                values: new object[] { 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "SharedPayments",
                columns: new[] { "IdSharedPayment", "IdFriend", "IdPayment" },
                values: new object[] { 2, 1, 4 });

            migrationBuilder.InsertData(
                table: "SharedPayments",
                columns: new[] { "IdSharedPayment", "IdFriend", "IdPayment" },
                values: new object[] { 3, 3, 10 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SharedPayments",
                keyColumn: "IdSharedPayment",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SharedPayments",
                keyColumn: "IdSharedPayment",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SharedPayments",
                keyColumn: "IdSharedPayment",
                keyValue: 3);
        }
    }
}
