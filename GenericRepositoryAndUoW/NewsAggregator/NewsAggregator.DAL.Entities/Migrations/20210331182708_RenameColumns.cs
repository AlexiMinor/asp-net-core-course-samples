using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsAggregator.DAL.Core.Migrations
{
    public partial class RenameColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body2",
                table: "News",
                newName: "Summary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Summary",
                table: "News",
                newName: "Body2");
        }
    }
}
