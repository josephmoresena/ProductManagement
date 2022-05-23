
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductManagement.Infrastructure.Migrations
{
    public partial class activeIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Product_Active",
                schema: "ProductManagement",
                table: "Product",
                column: "Active");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_Active",
                schema: "ProductManagement",
                table: "Product");
        }
    }
}
