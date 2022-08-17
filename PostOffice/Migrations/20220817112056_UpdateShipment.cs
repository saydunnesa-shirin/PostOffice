using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostOffice.Api.Migrations
{
    public partial class UpdateShipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shipments");
        }
    }
}
