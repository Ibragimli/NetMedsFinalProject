using Microsoft.EntityFrameworkCore.Migrations;

namespace NetMedsFull.Migrations
{
    public partial class sbilabupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "SbiLabs",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controller",
                table: "SbiLabs",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "SbiLabs");

            migrationBuilder.DropColumn(
                name: "Controller",
                table: "SbiLabs");
        }
    }
}
