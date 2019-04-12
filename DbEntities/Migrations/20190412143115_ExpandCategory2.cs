using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbEntities.Migrations
{
    public partial class ExpandCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "EnName");

            migrationBuilder.AddColumn<string>(
                name: "BgName",
                table: "Category",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeName",
                table: "Category",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbnail",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgName",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "DeName",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "EnName",
                table: "Category",
                newName: "Name");
        }
    }
}
