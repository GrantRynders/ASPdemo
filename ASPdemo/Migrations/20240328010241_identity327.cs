using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class identity327 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /**migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WalletAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PortfolioValue = table.Column<double>(type: "REAL", nullable: false),
                    UserId = table.Column<int>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio", x => x.PortfolioId);
                });
            migrationBuilder.CreateTable(
                name: "UsersRoles",
                columns: table => new
                {
                    UsersRolesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersroles", x => x.UsersRolesId);
                });
            migrationBuilder.AddPrimaryKey(
                name: "PK_portfolio",
                table: "Portfolios",
                column: "PortfolioId");


            //           ADDITIONS
            migrationBuilder.AddForeignKey( //Users to Portfolio
                name: "UserId",
                table: "Portfolios",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
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
                onDelete: ReferentialAction.SetNull);**/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropPrimaryKey(
            //     name: "PK_portfolio",
            //     table: "Portfolios");


            // migrationBuilder.AlterColumn<int>(
            //     name: "PortfolioId",
            //     table: "Portfolios",
            //     type: "INTEGER",
            //     nullable: false,
            //     oldClrType: typeof(int),
            //     oldType: "INTEGER")
            //     .OldAnnotation("Sqlite:Autoincrement", true);

            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "Portfolios",
            //     type: "INTEGER",
            //     nullable: false,
            //     oldClrType: typeof(int),
            //     oldType: "INTEGER")
            //     .Annotation("Sqlite:Autoincrement", true);

            // migrationBuilder.AddPrimaryKey(
            //     name: "PK_portfolio",
            //     table: "Portfolios",
            //     column: "PortfolioId");
        }
    }
}
