using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class updatevalues2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
