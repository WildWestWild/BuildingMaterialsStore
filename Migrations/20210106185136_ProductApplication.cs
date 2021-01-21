using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildingMaterialsStore.Migrations
{
    public partial class ProductApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationTypeId",
                table: "Product",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ApplicationTypeId",
                table: "Product",
                column: "ApplicationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ApplicationTypes_ApplicationTypeId",
                table: "Product",
                column: "ApplicationTypeId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ApplicationTypes_ApplicationTypeId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ApplicationTypeId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ApplicationTypeId",
                table: "Product");
        }
    }
}
