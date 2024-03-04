using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class MockupFriendTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Friends",
                columns: new[] { "IdFriend", "DateAdded", "FriendTag", "IdUser" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, 1 },
                    { 2, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1 },
                    { 3, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 2 },
                    { 4, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, 3 },
                    { 5, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1 },
                    { 6, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 6);
        }
    }
}
