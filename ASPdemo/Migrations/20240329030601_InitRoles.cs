using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class InitRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
name: "UsersRoles",
columns: table => new
{
UsersRolesId = table.Column<int>(nullable: false)
.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
UserId = table.Column<string>(nullable: false),
RoleId = table.Column<string>(nullable: false),

});

            /**migrationBuilder.AddPrimaryKey(
    name: "PK_portfolio",
    table: "Portfolios",
    column: "PortfolioId");
            **/

            //           ADDITIONS
            /**migrationBuilder.AddForeignKey( //Users to Portfolio
                name: "UserId",
                table: "Portfolios",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);**/
            migrationBuilder.AddForeignKey( //Roles to UsersRoles
                name: "RoleId",
                table: "UsersRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey( //Users to UsersRoles
                name: "UserId",
                table: "UsersRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey( //Roles to IdentityRoleClaim
                name: "RoleId",
                table: "IdentityRoleClaim",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
            migrationBuilder.AddForeignKey( //Users to IdentityUserClaim
                name: "UserId",
                table: "IdentityUserClaim",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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
