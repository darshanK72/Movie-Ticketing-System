 using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(string id);
        Task<bool> CreateMovieAsync(Movie movie,IEnumerable<string> genreIds,IEnumerable<string> languageIds);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(string id);
        Task<IEnumerable<Movie>> GetActiveMoviesAsync();
        Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre);
        Task<IEnumerable<Movie>> GetMoviesByLanguageAsync(string language);
        Task<IEnumerable<Movie>> GetMoviesByRatingAsync(CertificateRating rating);
    }
}