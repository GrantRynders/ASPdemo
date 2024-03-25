using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class PortfolioCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "portfolio",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WalletAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PortfolioValue = table.Column<double>(type: "REAL", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolio", x => x.PortfolioId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "portfolio");
        }
    }
}
