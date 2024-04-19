using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class conversions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Conversions",
                columns: table => new
                {
                    ConversionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pair1 = table.Column<string>(type: "TEXT", nullable: true),
                    Pair2 = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PercentChange24Hr = table.Column<double>(type: "REAL", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    Volume24 = table.Column<double>(type: "REAL", nullable: true),
                    PercentChange1hr = table.Column<double>(type: "REAL", nullable: true),
                    PercentChange7d = table.Column<double>(type: "REAL", nullable: true),
                    MarketCap = table.Column<double>(type: "REAL", nullable: true),
                    TotalSupply = table.Column<double>(type: "REAL", nullable: true),
                    SecondPercentChange24Hr = table.Column<double>(type: "REAL", nullable: true),
                    SecondDescription = table.Column<string>(type: "TEXT", nullable: true),
                    SecondPrice = table.Column<double>(type: "REAL", nullable: true),
                    SecondVolume24 = table.Column<double>(type: "REAL", nullable: true),
                    SecondPercentChange1hr = table.Column<double>(type: "REAL", nullable: true),
                    SecondPercentChange7d = table.Column<double>(type: "REAL", nullable: true),
                    SecondMarketCap = table.Column<double>(type: "REAL", nullable: true),
                    SecondTotalSupply = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversions", x => x.ConversionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversions");
        }
    }
}
