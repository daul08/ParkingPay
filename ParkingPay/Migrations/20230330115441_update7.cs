using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingPay.Migrations
{
    /// <inheritdoc />
    public partial class update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPayment",
                table: "EnteredCarModel",
                newName: "IsPaid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPaid",
                table: "EnteredCarModel",
                newName: "IsPayment");
        }
    }
}
