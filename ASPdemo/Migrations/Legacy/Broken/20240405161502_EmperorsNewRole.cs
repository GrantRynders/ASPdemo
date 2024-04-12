using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class EmperorsNewRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Categories_CategoryId",
                table: "Currencies");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRoles_Roles_RoleId",
                table: "UsersRoles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_CategoryId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Currencies");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoryCurrency",
                columns: table => new
                {
                    CategoriesCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoinsCurrencyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCurrency", x => new { x.CategoriesCategoryId, x.CoinsCurrencyId });
                    table.ForeignKey(
                        name: "FK_CategoryCurrency_Categories_CategoriesCategoryId",
                        column: x => x.CategoriesCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCurrency_Currencies_CoinsCurrencyId",
                        column: x => x.CoinsCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NormalizedName = table.Column<string>(type: "TEXT", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityRole_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesPortfolios_CurrencyId",
                table: "CurrenciesPortfolios",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesPortfolios_PortfolioId",
                table: "CurrenciesPortfolios",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesCategories_CategoryId",
                table: "CurrenciesCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesCategories_CurrencyId",
                table: "CurrenciesCategories",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCurrency_CoinsCurrencyId",
                table: "CategoryCurrency",
                column: "CoinsCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityRole_UserId",
                table: "IdentityRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesCategories_Categories_CategoryId",
                table: "CurrenciesCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesCategories_Currencies_CurrencyId",
                table: "CurrenciesCategories",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesPortfolios_Currencies_CurrencyId",
                table: "CurrenciesPortfolios",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "CurrencyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesPortfolios_Portfolios_PortfolioId",
                table: "CurrenciesPortfolios",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_IdentityRole_Id",
                table: "Roles",
                column: "Id",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRoles_IdentityRole_RoleId",
                table: "UsersRoles",
                column: "RoleId",
                principalTable: "IdentityRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesCategories_Categories_CategoryId",
                table: "CurrenciesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesCategories_Currencies_CurrencyId",
                table: "CurrenciesCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesPortfolios_Currencies_CurrencyId",
                table: "CurrenciesPortfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesPortfolios_Portfolios_PortfolioId",
                table: "CurrenciesPortfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_IdentityRole_Id",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRoles_IdentityRole_RoleId",
                table: "UsersRoles");

            migrationBuilder.DropTable(
                name: "CategoryCurrency");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_CurrenciesPortfolios_CurrencyId",
                table: "CurrenciesPortfolios");

            migrationBuilder.DropIndex(
                name: "IX_CurrenciesPortfolios_PortfolioId",
                table: "CurrenciesPortfolios");

            migrationBuilder.DropIndex(
                name: "IX_CurrenciesCategories_CategoryId",
                table: "CurrenciesCategories");

            migrationBuilder.DropIndex(
                name: "IX_CurrenciesCategories_CurrencyId",
                table: "CurrenciesCategories");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "Roles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Roles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Roles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Roles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Currencies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CategoryId",
                table: "Currencies",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Categories_CategoryId",
                table: "Currencies",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

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
        }
    }
}
