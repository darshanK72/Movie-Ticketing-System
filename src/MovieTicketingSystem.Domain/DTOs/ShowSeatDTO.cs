using System;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class ShowSeatDTO
    {
        public string? Id { get; set; }
        public string? ShowId { get; set; }
        public string? SeatId { get; set; }
        public string? SeatNumber { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public string? SeatType { get; set; }
        public decimal PriceMultiplier { get; set; }
        public bool IsBooked { get; set; }
        public string? BookingId { get; set; }
        public string? BookingStatus { get; set; }
    }
}


