using Microsoft.EntityFrameworkCore.Migrations;

namespace NetMedsFull.Migrations
{
    public partial class SubcategoryBrandCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_SubCategoryId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Brands");

            migrationBuilder.CreateTable(
                name: "SubCategoryBrands",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDelete = table.Column<bool>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    SubCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategoryBrands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategoryBrands_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubCategoryBrands_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryBrands_BrandId",
                table: "SubCategoryBrands",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategoryBrands_SubCategoryId",
                table: "SubCategoryBrands",
                column: "SubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubCategoryBrands");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Brands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_SubCategoryId",
                table: "Brands",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_SubCategories_SubCategoryId",
                table: "Brands",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
