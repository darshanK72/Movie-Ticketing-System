using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Queries.Search
{
    public class SearchQueryHandler : IRequestHandler<SearchQuery, SearchResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ITheaterRepository _theaterRepository;

        public SearchQueryHandler(
            IMovieRepository movieRepository,
            ITheaterRepository theaterRepository)
        {
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
        }

        public async Task<SearchResponse> Handle(SearchQuery request, CancellationToken cancellationToken)
        {
            var searchTerm = request.SearchTerm?.ToLower() ?? string.Empty;
            var location = request.Location?.ToLower() ?? string.Empty;
            var language = request.Language?.ToLower() ?? string.Empty;
            var genre = request.Genre?.ToLower() ?? string.Empty;
            var movieFilter = request.Movie?.ToLower() ?? string.Empty;
            var theaterFilter = request.Theater?.ToLower() ?? string.Empty;

            var hasMovieSearchCriteria = !string.IsNullOrEmpty(searchTerm) || 
                                       !string.IsNullOrEmpty(movieFilter) || 
                                       !string.IsNullOrEmpty(language) || 
                                       !string.IsNullOrEmpty(genre);

            var hasTheaterSearchCriteria = !string.IsNullOrEmpty(searchTerm) || 
                                         !string.IsNullOrEmpty(location) || 
                                         !string.IsNullOrEmpty(theaterFilter);

            var movies = hasMovieSearchCriteria ? await _movieRepository.GetAllMoviesAsync() : Enumerable.Empty<Movie>();
            var filteredMovies = movies.Where(m =>
                (string.IsNullOrEmpty(searchTerm) || 
                 m.Title!.ToLower().Contains(searchTerm) ||
                 m.Description!.ToLower().Contains(searchTerm)) &&
                (string.IsNullOrEmpty(movieFilter) ||
                 m.Title!.ToLower().Contains(movieFilter)) &&
                (string.IsNullOrEmpty(language) ||
                 m.Languages!.Any(l => l.Name!.ToLower().Contains(language))) &&
                (string.IsNullOrEmpty(genre) ||
                 m.Genres!.Any(g => g.Name!.ToLower().Contains(genre))))
                .Select(m => new MovieDTO
                {
                    Id = m.Id.ToString(),
                    Title = m.Title,
                    Description = m.Description,
                    DurationInMinutes = m.DurationInMinutes,
                    ReleaseDate = m.ReleaseDate,
                    ViewerRating = m.ViewerRating,
                    Languages = m.Languages.Select(l => l.Name).ToList(),
                    Genres = m.Genres?.Select(g => g.Name).ToList(),
                    CertificateRating = m.CertificateRating.ToString(),
                    Director = m.Director,
                    PosterUrl = m.PosterUrl,
                    TrailerUrl = m.TrailerUrl
                })
                .ToList();

            var theaters = hasTheaterSearchCriteria ? await _theaterRepository.GetAllTheatersAsync() : Enumerable.Empty<Theater>();
            var filteredTheaters = theaters.Where(t =>
                (string.IsNullOrEmpty(searchTerm) ||
                 t.Name!.ToLower().Contains(searchTerm) ||
                 t.Description!.ToLower().Contains(searchTerm)) &&
                (string.IsNullOrEmpty(location) ||
                 t.Address!.City!.ToLower().Contains(location) ||
                 t.Address!.State!.ToLower().Contains(location)) &&
                (string.IsNullOrEmpty(theaterFilter) ||
                 t.Name!.ToLower().Contains(theaterFilter)))
                .Select(t => new TheaterDTO
                {
                    Id = t.Id.ToString(),
                    Name = t.Name,
                    Description = t.Description,
                    ContactNumber = t.ContactNumber,
                    Email = t.Email,
                    Website = t.Website,
                    Address = new AddressDTO
                    {
                        Id = t.Address!.Id.ToString(),
                        Street = t.Address.Street,
                        City = t.Address.City,
                        State = t.Address.State,
                        Country = t.Address.Country,
                        PostalCode = t.Address.PostalCode,
                        Details = t.Address.Details
                    }
                })
                .ToList();

            return new SearchResponse
            {
                Movies = filteredMovies,
                Theaters = filteredTheaters
            };
        }
    }
} 