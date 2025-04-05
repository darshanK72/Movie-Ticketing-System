using System;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class MovieDTO
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public string? Language { get; set; }
        public int DurationInMinutes { get; set; }
        public string? Director { get; set; }
        public string? Cast { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public MovieRating Rating { get; set; }
        public bool IsActive { get; set; }
    }
} 