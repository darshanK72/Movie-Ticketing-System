using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Search
{
    public class SearchQuery : IRequest<SearchResponse>
    {
        public string? SearchTerm { get; set; }
        public string? Location { get; set; }
        public string? Genre {get;set;}
        public string? Language {get;set;}
        public string? Movie { get; set; }
        public string? Theater { get; set; }
    }
} 