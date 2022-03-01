using Microsoft.EntityFrameworkCore.Migrations;

namespace NetMedsFull.Migrations
{
    public partial class LabTestAppUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "LabTests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabTests_AppUserId",
                table: "LabTests",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabTests_AspNetUsers_AppUserId",
                table: "LabTests",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabTests_AspNetUsers_AppUserId",
                table: "LabTests");

            migrationBuilder.DropIndex(
                name: "IX_LabTests_AppUserId",
                table: "LabTests");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "LabTests");
        }
    }
}
