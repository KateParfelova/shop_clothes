using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop_clothes.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_Categoryid",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Categoryid",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Categoryid",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_category",
                table: "Products",
                column: "category");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_category",
                table: "Products",
                column: "category",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_category",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_category",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "Categoryid",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Categoryid",
                table: "Products",
                column: "Categoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_Categoryid",
                table: "Products",
                column: "Categoryid",
                principalTable: "Categories",
                principalColumn: "id");
        }
    }
}
