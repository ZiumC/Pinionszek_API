using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class FixedReferencesBetweenPaymentsAndSharedPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SharedPayments_IdPayment",
                table: "SharedPayments");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPayments_IdPayment",
                table: "SharedPayments",
                column: "IdPayment",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SharedPayments_IdPayment",
                table: "SharedPayments");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPayments_IdPayment",
                table: "SharedPayments",
                column: "IdPayment");
        }
    }
}
