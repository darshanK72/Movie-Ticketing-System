using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketingSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingStatus",
                table: "ShowSeats",
                newName: "SeatBookingStatus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatBookingStatus",
                table: "ShowSeats",
                newName: "BookingStatus");
        }
    }
}
