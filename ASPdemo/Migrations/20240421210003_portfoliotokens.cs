using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class portfoliotokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Portfolios_PortfolioId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_PortfolioId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Currencies");

            migrationBuilder.CreateTable(
                name: "PortfolioToken",
                columns: table => new
                {
                    PortfolioTokenId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TokenAmount = table.Column<string>(type: "TEXT", nullable: false),
                    TokenName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioToken", x => x.PortfolioTokenId);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "028394ee-9773-49bf-8e08-12e2d437ff38", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PermissionsLevel", "PhoneNumber", "PhoneNumberConfirmed", "PortfolioId", "SecurityStamp", "TwoFactorEnabled", "UserId", "UserName" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "687aebbf-7942-4c6a-9af7-95961fdb0444", "grantrynders@outlook.com", true, false, null, "GRANTRYNDERS@OUTLOOK.COM", "ADMIN", "AQAAAAIAAYagAAAAEGsr+mgDTBwmkKfM0SZkp6CRhllNp4+t7egOiHDPxNYEtPCWmUBZ/vhp4TIUo5aibg==", 0, null, false, 0, "d7dfa50e-bf5d-466b-a7d1-a60efc66f8e0", false, 0, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioToken");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2c5e174e-3b0e-446f-86af-483d56fd7210", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Currencies",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_PortfolioId",
                table: "Currencies",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Portfolios_PortfolioId",
                table: "Currencies",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId");
        }
    }
}
