using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceMonitoring.Data.Migrations
{
    public partial class CreateWebsiteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WebsiteId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Migros" });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "A101" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_WebsiteId",
                table: "Products",
                column: "WebsiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Websites_WebsiteId",
                table: "Products",
                column: "WebsiteId",
                principalTable: "Websites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Websites_WebsiteId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Websites");

            migrationBuilder.DropIndex(
                name: "IX_Products_WebsiteId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WebsiteId",
                table: "Products");
        }
    }
}
