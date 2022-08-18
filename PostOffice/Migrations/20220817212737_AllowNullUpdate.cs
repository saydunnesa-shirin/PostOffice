using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostOffice.Api.Migrations
{
    public partial class AllowNullUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Shipments_ShipmentId",
                table: "Bags");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Bags_BagId",
                table: "Parcels");

            migrationBuilder.AlterColumn<int>(
                name: "BagId",
                table: "Parcels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "Bags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Shipments_ShipmentId",
                table: "Bags",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Bags_BagId",
                table: "Parcels",
                column: "BagId",
                principalTable: "Bags",
                principalColumn: "BagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Shipments_ShipmentId",
                table: "Bags");

            migrationBuilder.DropForeignKey(
                name: "FK_Parcels_Bags_BagId",
                table: "Parcels");

            migrationBuilder.AlterColumn<int>(
                name: "BagId",
                table: "Parcels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShipmentId",
                table: "Bags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Shipments_ShipmentId",
                table: "Bags",
                column: "ShipmentId",
                principalTable: "Shipments",
                principalColumn: "ShipmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parcels_Bags_BagId",
                table: "Parcels",
                column: "BagId",
                principalTable: "Bags",
                principalColumn: "BagId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
