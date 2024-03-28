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
            migrationBuilder.DropPrimaryKey(
                name: "PK_portfolio",
                table: "portfolio");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "portfolio",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "portfolio",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "portfolio",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_portfolio",
                table: "portfolio",
                column: "PortfolioId");


            //           ADDITIONS
            migrationBuilder.AddForeignKey( //Users to Portfolio
                name: "UserId",
                table: "Portfolio",
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
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_portfolio",
                table: "portfolio");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "portfolio",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "portfolio",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "portfolio",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_portfolio",
                table: "portfolio",
                column: "Id");
        }
    }
}
