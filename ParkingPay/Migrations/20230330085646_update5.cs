using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingPay.Migrations
{
    /// <inheritdoc />
    public partial class update5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amaount",
                table: "EnteredCarModel",
                newName: "Paid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Paid",
                table: "EnteredCarModel",
                newName: "Amaount");
        }
    }
}
