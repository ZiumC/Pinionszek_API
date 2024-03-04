using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class AddedReferencesBetweenPaymentPaymentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPaymentStatus",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IdPaymentStatus",
                table: "Payments",
                column: "IdPaymentStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentStatuses_IdPaymentStatus",
                table: "Payments",
                column: "IdPaymentStatus",
                principalTable: "PaymentStatuses",
                principalColumn: "IdPaymentStatus",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentStatuses_IdPaymentStatus",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_IdPaymentStatus",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IdPaymentStatus",
                table: "Payments");
        }
    }
}
