using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostOffice.Api.Migrations
{
    public partial class UpdateBagTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Shipments_ShipmentId",
                table: "Bags");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Shipments_ShipmentId",
                table: "Bags");

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
        }
    }
}
