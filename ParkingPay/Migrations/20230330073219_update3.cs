using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingPay.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Amaount",
                table: "EnteredCarModel",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amaount",
                table: "EnteredCarModel");
        }
    }
}
