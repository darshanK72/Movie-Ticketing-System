using System;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class ShowTimingDTO
    {
        public string? Id { get; set; }
        public string? ShowId { get; set; }
        public string? ShowManagerId { get; set; }
        public string? ShowManagerName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal BasePrice { get; set; }
        public string? ShowStatus { get; set; }
        public ICollection<BookingDTO>? Bookings { get; set; }
    }
} 