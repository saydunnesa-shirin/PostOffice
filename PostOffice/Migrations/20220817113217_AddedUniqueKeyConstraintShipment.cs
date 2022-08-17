using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostOffice.Api.Migrations
{
    public partial class AddedUniqueKeyConstraintShipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_ShipmentNumber",
                table: "Shipments",
                column: "ShipmentNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_ShipmentNumber",
                table: "Shipments");
        }
    }
}
