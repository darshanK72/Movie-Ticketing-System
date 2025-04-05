using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class CinemaHallDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int TotalSeats { get; set; }
        public int NumberOfRows { get; set; }
        public int SeatsPerRow { get; set; }
        public bool Has3D { get; set; }
        public bool HasDolby { get; set; }
        public string? TheaterId { get; set; }
        public ICollection<SeatDTO>? Seats { get; set; }
    }
} 