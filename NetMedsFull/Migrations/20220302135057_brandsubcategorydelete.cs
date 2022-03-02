using Microsoft.EntityFrameworkCore.Migrations;

namespace NetMedsFull.Migrations
{
    public partial class brandsubcategorydelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "Brands",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "Brands",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
