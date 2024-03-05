using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class FixedMockupDataInFriendTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 5,
                column: "FriendTag",
                value: 1003);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Friends",
                keyColumn: "IdFriend",
                keyValue: 5,
                column: "FriendTag",
                value: 1004);
        }
    }
}
