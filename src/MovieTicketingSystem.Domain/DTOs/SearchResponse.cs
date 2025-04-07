using System.Collections.Generic;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class SearchResponse
    {
        public IEnumerable<MovieDTO>? Movies { get; set; }
        public IEnumerable<TheaterDTO>? Theaters { get; set; }
    }
} 