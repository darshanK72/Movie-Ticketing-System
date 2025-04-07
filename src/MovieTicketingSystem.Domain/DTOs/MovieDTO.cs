using System;
using System.Collections.Generic;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class MovieDTO
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<string>? Genres { get; set; }
        public List<string>? Languages { get; set; }
        public int DurationInMinutes { get; set; }
        public string? Director { get; set; }
        public string? PosterUrl { get; set; }
        public string? TrailerUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? CertificateRating { get; set; }
        public double? ViewerRating {  get; set; }
        public ICollection<ShowDTO>? Shows {get;set;}
    }
} 