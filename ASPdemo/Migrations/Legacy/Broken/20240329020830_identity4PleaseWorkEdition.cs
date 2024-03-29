using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class identity4PleaseWorkEdition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Roles",
                type: "TEXT",
                nullable: true);
            migrationBuilder.DropTable(
                name: "IdentityRoleClaim"
            );
            // migrationBuilder.CreateTable(
            //     name: "IdentityRoleClaim",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "INTEGER", nullable: false)
            //             .Annotation("Sqlite:Autoincrement", true),
            //         RoleId = table.Column<string>(type: "TEXT", nullable: false),
            //         ClaimType = table.Column<string>(type: "TEXT", nullable: true),
            //         ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_IdentityRoleClaim", x => x.Id);
            //     });
            migrationBuilder.CreateIndex(
                name: "IX_UsersRoles_RoleId",
                table: "UsersRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersRoles_UserId",
                table: "UsersRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserId",
                table: "Roles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRoles_Roles_RoleId",
                table: "UsersRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRoles_Users_UserId",
                table: "UsersRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRoles_Roles_RoleId",
                table: "UsersRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRoles_Users_UserId",
                table: "UsersRoles");

            migrationBuilder.DropIndex(
                name: "IX_UsersRoles_RoleId",
                table: "UsersRoles");

            migrationBuilder.DropIndex(
                name: "IX_UsersRoles_UserId",
                table: "UsersRoles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Roles");
        }
    }
}
