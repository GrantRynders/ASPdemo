using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class PortfoliosFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PortfolioValue",
                table: "Portfolios",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Currencies",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_PortfolioId",
                table: "Currencies",
                column: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencies_Portfolios_PortfolioId",
                table: "Currencies",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencies_Portfolios_PortfolioId",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_PortfolioId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Currencies");

            migrationBuilder.AlterColumn<double>(
                name: "PortfolioValue",
                table: "Portfolios",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);
        }
    }
}
