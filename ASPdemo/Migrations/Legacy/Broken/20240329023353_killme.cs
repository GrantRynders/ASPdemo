using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class killme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim",
                table: "IdentityRoleClaim");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim",
                newName: "IdentityRoleClaim<string>");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim<string>",
                newName: "IdentityRoleClaim");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim",
                table: "IdentityRoleClaim",
                column: "Id");
        }
    }
}
