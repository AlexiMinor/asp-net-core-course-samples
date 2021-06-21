using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsAggregator.DAL.Core.Migrations
{
    public partial class ChangeRefreshTokenDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresUtcDateTime",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false
            );

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateDateTime",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false);

            migrationBuilder.DropColumn(
                name: "ExpiresUtc",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(name: "ExpiresUtcDateTime",
                table: "RefreshTokens", newName: "ExpiresUtc");


            migrationBuilder.RenameColumn(name: "CreationDateDateTime",
                table: "RefreshTokens", newName: "CreationDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ExpiresUtc",
                table: "RefreshTokens",
                type: "float",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<double>(
                name: "CreationDate",
                table: "RefreshTokens",
                type: "float",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
