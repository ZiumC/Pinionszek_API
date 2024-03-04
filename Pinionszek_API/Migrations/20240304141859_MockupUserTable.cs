using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "IdUser", "BlockedTo", "Email", "Login", "LoginAttempts", "Password", "PasswordSalt", "RefreshToken", "RegisteredAt", "UserTag" },
                values: new object[,]
                {
                    { 1, null, "test1@test.pl", "test1", 0, "password1", "passsalt", null, new DateTime(2024, 3, 4, 15, 18, 58, 857, DateTimeKind.Local).AddTicks(6330), 1001 },
                    { 2, null, "test2@test.pl", "test2", 0, "password2", "passsalt", null, new DateTime(2023, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002 },
                    { 3, null, "test3@test.pl", "test3", 0, "password3", "passsalt", null, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003 },
                    { 4, null, "test4@test.pl", "test4", 0, "password4", "passsalt", null, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: 4);
        }
    }
}
