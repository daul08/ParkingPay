using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingPay.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParkingCost",
                table: "EnteredCarModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingTimeinMinutes",
                table: "EnteredCarModel",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingCost",
                table: "EnteredCarModel");

            migrationBuilder.DropColumn(
                name: "ParkingTimeinMinutes",
                table: "EnteredCarModel");
        }
    }
}
