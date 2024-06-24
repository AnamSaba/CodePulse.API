using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodePusle.API.Migrations
{
    /// <inheritdoc />
    public partial class Restoringdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Urlhandle",
                table: "Categories",
                newName: "UrlHandle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlHandle",
                table: "Categories",
                newName: "Urlhandle");
        }
    }
}
