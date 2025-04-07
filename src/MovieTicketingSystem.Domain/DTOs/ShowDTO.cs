using System;
using System.Collections.Generic;
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
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<ShowTimingDTO>? ShowTimings { get; set; }
    }
} 