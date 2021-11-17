using Microsoft.EntityFrameworkCore.Migrations;

namespace PriceMonitoring.Data.Migrations
{
    public partial class CreateTableProductSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    ProductId = table.Column<int>(type: "int", maxLength: 5, nullable: false),
                    ProductPriceId = table.Column<int>(type: "int", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubscriptions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSubscriptions");
        }
    }
}
