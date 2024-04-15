using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class removecoins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Categories_CategoryId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_CategoryId",
                table: "Currencies");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Currencies",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Currencies",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

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
    }
}
