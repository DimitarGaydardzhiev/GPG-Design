using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DbEntities.Migrations
{
    public partial class ImageDescriptionsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "GalleryImage",
                newName: "EnDescription");

            migrationBuilder.AddColumn<string>(
                name: "BgDescription",
                table: "GalleryImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryDescription",
                table: "GalleryImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeDescription",
                table: "GalleryImage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BgDescription",
                table: "GalleryImage");

            migrationBuilder.DropColumn(
                name: "CategoryDescription",
                table: "GalleryImage");

            migrationBuilder.DropColumn(
                name: "DeDescription",
                table: "GalleryImage");

            migrationBuilder.RenameColumn(
                name: "EnDescription",
                table: "GalleryImage",
                newName: "Description");
        }
    }
}
