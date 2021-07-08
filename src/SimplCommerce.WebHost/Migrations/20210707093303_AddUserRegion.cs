using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SimplCommerce.WebHost.Migrations
{
    public partial class AddUserRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Core_UserRegion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<string>(type: "text", nullable: true),
                    CreateOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core_UserRegion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Core_UserRegion_Core_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Core_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Core_UserRegion_Core_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Core_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserRegion_CountryId",
                table: "Core_UserRegion",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Core_UserRegion_UserId",
                table: "Core_UserRegion",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Core_UserRegion");
        }
    }
}
