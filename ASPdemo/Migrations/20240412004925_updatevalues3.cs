using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPdemo.Migrations
{
    /// <inheritdoc />
    public partial class updatevalues3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CMCCategoryId",
                table: "Categories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CMCCategoryId",
                table: "Categories");
        }
    }
}
