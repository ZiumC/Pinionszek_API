using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class AddedCollumnsToPaymentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaidOn",
                table: "Payments",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentAddedOn",
                table: "Payments",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentAddedOn",
                table: "Payments");
        }
    }
}
