using System;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class ShowDTO
    {
        public string? Id { get; set; }
        public string? MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public string? CinemaHallId { get; set; }
        public string? CinemaHallName { get; set; }
        public string? TheaterId { get; set; }
        public string? TheaterName { get; set; }
        public string? ShowManagerId { get; set; }
        public string? ShowManagerName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public decimal BasePrice { get; set; }
        public ShowStatus Status { get; set; }
        public ICollection<BookingDTO>? Bookings {get;set;}
    }
} 