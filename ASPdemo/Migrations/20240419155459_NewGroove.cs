using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class NewGroove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim",
                table: "IdentityUserClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim",
                table: "IdentityRoleClaim");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim",
                newName: "IdentityUserClaim<string>");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim",
                newName: "IdentityRoleClaim<string>");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim<string>",
                table: "IdentityUserClaim<string>",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim<string>",
                table: "IdentityUserClaim<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim<string>",
                newName: "IdentityUserClaim");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim<string>",
                newName: "IdentityRoleClaim");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim",
                table: "IdentityUserClaim",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim",
                table: "IdentityRoleClaim",
                column: "Id");
        }
    }
}
