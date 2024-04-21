using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class portfoliotokens2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PortfolioToken",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "a94c3698-13e4-4c98-ba3a-be341f99f3c9");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7b2167f2-1fc8-4245-89f2-837ef86c3408", "AQAAAAIAAYagAAAAEA+4RxxlhWrUAsrpZ09k+etnk+XcOo2I9jEHM711r/UPXSlMeu9NZiAtcuvk2OUfxw==", "b7cb266c-a44a-4c0f-9fa7-e3e2adf3a1d5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PortfolioToken",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "028394ee-9773-49bf-8e08-12e2d437ff38");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "687aebbf-7942-4c6a-9af7-95961fdb0444", "AQAAAAIAAYagAAAAEGsr+mgDTBwmkKfM0SZkp6CRhllNp4+t7egOiHDPxNYEtPCWmUBZ/vhp4TIUo5aibg==", "d7dfa50e-bf5d-466b-a7d1-a60efc66f8e0" });
        }
    }
}
