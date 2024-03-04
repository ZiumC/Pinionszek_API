using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class CreatedTableSharedPayments_AddedReferencesBetweenPaymentSharedPaymentFriendTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SharedPayments",
                columns: table => new
                {
                    IdSharedPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPayment = table.Column<int>(type: "int", nullable: false),
                    IdFriend = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedPayments", x => x.IdSharedPayment);
                    table.ForeignKey(
                        name: "FK_SharedPayments_Friends_IdFriend",
                        column: x => x.IdFriend,
                        principalTable: "Friends",
                        principalColumn: "IdFriend",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedPayments_Payments_IdPayment",
                        column: x => x.IdPayment,
                        principalTable: "Payments",
                        principalColumn: "IdPayment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedPayments_IdFriend",
                table: "SharedPayments",
                column: "IdFriend");

            migrationBuilder.CreateIndex(
                name: "IX_SharedPayments_IdPayment",
                table: "SharedPayments",
                column: "IdPayment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedPayments");
        }
    }
}
