﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM_AppWeb_Bicycle_Shop.Migrations
{
    /// <inheritdoc />
    public partial class initpppppp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewsContent",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsContent",
                table: "News");
        }
    }
}
