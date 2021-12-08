using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceMonitoring.Data.Migrations
{
    public partial class AddDemoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsConfirm", "LastName", "Password", "Token" },
                values: new object[] { 1, "demo@demo.com", "Demo", true, "User", "$2a$11$sZKWOyPMBoLzwdlWh3QTKuvj27fovnTfC2QLSd8V8Vtvg.a6vYcly", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
