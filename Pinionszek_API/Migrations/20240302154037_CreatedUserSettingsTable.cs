using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pinionszek_API.Migrations
{
    public partial class CreatedUserSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    IdUserSetting = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UseBudgetRules = table.Column<bool>(type: "bit", nullable: false),
                    DisplayBudgetRules = table.Column<bool>(type: "bit", nullable: false),
                    Needs = table.Column<decimal>(type: "money", nullable: false),
                    Wants = table.Column<decimal>(type: "money", nullable: false),
                    Savings = table.Column<decimal>(type: "money", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.IdUserSetting);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_IdUser",
                table: "UserSettings",
                column: "IdUser",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
