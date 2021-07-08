using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplCommerce.WebHost.Migrations
{
    public partial class AddMoreColumnsToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerAccount",
                table: "Core_User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Core_User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Core_User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesResponsible",
                table: "Core_User",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerAccount",
                table: "Core_User");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Core_User");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Core_User");

            migrationBuilder.DropColumn(
                name: "SalesResponsible",
                table: "Core_User");
        }
    }
}
