using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM_AppWeb_Bicycle_Shop.Migrations
{
    /// <inheritdoc />
    public partial class CartInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "Shop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalRevenue",
                table: "Shop",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
