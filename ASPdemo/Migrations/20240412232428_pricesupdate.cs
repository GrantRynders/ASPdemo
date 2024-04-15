using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class pricesupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarketCap",
                table: "Currencies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PercentChange1hr",
                table: "Currencies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PercentChange7d",
                table: "Currencies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalSupply",
                table: "Currencies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Volume24",
                table: "Currencies",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketCap",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "PercentChange1hr",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "PercentChange7d",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "TotalSupply",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Volume24",
                table: "Currencies");
        }
    }
}
