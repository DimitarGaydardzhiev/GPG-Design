using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbEntities.Migrations
{
    public partial class ExpandCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BgDescription",
                table: "Category",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeDescription",
                table: "Category",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EnDescription",
                table: "Category",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgDescription",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeDescription",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "EnDescription",
                table: "Category");
        }
    }
}
