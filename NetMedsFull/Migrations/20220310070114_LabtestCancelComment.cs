using Microsoft.EntityFrameworkCore.Migrations;

namespace NetMedsFull.Migrations
{
    public partial class LabtestCancelComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelComment",
                table: "LabTests",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelComment",
                table: "LabTests");
        }
    }
}
