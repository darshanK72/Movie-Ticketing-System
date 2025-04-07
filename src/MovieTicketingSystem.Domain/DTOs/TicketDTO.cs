using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class TicketDTO
    {
        public string BookingId { get; set; } = string.Empty;
        public string MovieTitle { get; set; } = string.Empty;
        public string TheaterName { get; set; } = string.Empty;
        public string CinemaHallName { get; set; } = string.Empty;
        public DateTime ShowDateTime { get; set; }
        public List<string> SeatNumbers { get; set; } = new();
        public int NumberOfTickets { get; set; }
        public decimal TotalAmount { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string? BookingStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime BookingDate { get; set; }
    }
} 