using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostOffice.Api.Migrations
{
    public partial class UniqueKeyConstraintToBagAndParcel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_ParcelNumber",
                table: "Parcels",
                column: "ParcelNumber");

            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_BagNumber",
                table: "Bags",
                column: "BagNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_ParcelNumber",
                table: "Parcels");

            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_BagNumber",
                table: "Bags");
        }
    }
}
