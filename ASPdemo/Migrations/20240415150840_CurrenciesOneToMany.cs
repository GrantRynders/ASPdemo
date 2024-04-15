using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class CurrenciesOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryCurrency");

            migrationBuilder.DropTable(
                name: "CurrenciesCategories");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Categories_CategoryId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_CategoryId",
                table: "Currencies");

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
                name: "CurrenciesCategories",
                columns: table => new
                {
                    CurrenciesCategoriesId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrenciesCategories", x => x.CurrenciesCategoriesId);
                    table.ForeignKey(
                        name: "FK_CurrenciesCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrenciesCategories_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCurrency_CoinsCurrencyId",
                table: "CategoryCurrency",
                column: "CoinsCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesCategories_CategoryId",
                table: "CurrenciesCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesCategories_CurrencyId",
                table: "CurrenciesCategories",
                column: "CurrencyId");
        }
    }
}
