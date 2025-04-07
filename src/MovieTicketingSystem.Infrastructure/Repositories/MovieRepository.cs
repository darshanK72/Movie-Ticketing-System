using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Persistence;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly TicketingDbContext _context;

        public MovieRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.Include(m => m.Genres).Include(m => m.Languages).ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(string id)
        {
            return await _context.Movies.Include(m => m.Genres).Include(m => m.Languages).FirstOrDefaultAsync(m => m.Id.ToString() == id);
        }

        public async Task<bool> CreateMovieAsync(Movie movie,IEnumerable<string> genreIds,IEnumerable<string> languageIds)
        {
            if (genreIds != null && genreIds.Any())
            {
                var genreGuids = genreIds.Select(id => Guid.Parse(id)).ToList();
                var genres = await _context.Genres.Where(g => genreGuids.Contains(g.Id)).ToListAsync();
                movie.Genres = genres;
            }

            if (languageIds != null && languageIds.Any())
            {
                var languageGuids = languageIds.Select(id => Guid.Parse(id)).ToList();
                var languages = await _context.Languages.Where(l => languageGuids.Contains(l.Id)).ToListAsync();
                movie.Languages = languages;
            }

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMovieAsync(Movie movie, IEnumerable<string> genreIds, IEnumerable<string> languageIds)
        {
            var existingMovie = await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Languages)
                .FirstOrDefaultAsync(m => m.Id == movie.Id);

            if (existingMovie == null)
                return false;

            // Update basic properties
            existingMovie.Title = movie.Title;
            existingMovie.Description = movie.Description;
            existingMovie.DurationInMinutes = movie.DurationInMinutes;
            existingMovie.Director = movie.Director;
            existingMovie.PosterUrl = movie.PosterUrl;
            existingMovie.TrailerUrl = movie.TrailerUrl;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.CertificateRating = movie.CertificateRating;
            existingMovie.ViewerRating = movie.ViewerRating;
            existingMovie.UpdatedAt = DateTime.UtcNow;

            // Update genres
            if (genreIds != null && genreIds.Any())
            {
                var genreGuids = genreIds.Select(id => Guid.Parse(id)).ToList();
                var genres = await _context.Genres.Where(g => genreGuids.Contains(g.Id)).ToListAsync();
                existingMovie.Genres = genres;
            }

            // Update languages
            if (languageIds != null && languageIds.Any())
            {
                var languageGuids = languageIds.Select(id => Guid.Parse(id)).ToList();
                var languages = await _context.Languages.Where(l => languageGuids.Contains(l.Id)).ToListAsync();
                existingMovie.Languages = languages;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMovieAsync(string id)
        {
            var movie = await _context.Movies.FindAsync(Guid.Parse(id));
            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Movie>> GetActiveMoviesAsync()
        {
            return await _context.Movies.Include(m => m.Genres).Include(m => m.Languages)
                .Where(m => m.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
        {
            return await _context.Movies.Include(m => m.Genres).Include(m => m.Languages)
                .Where(m => m.Genres!.Select(g => g.Name).Contains(genre) && m.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByLanguageAsync(string language)
        {
            return await _context.Movies.Include(m => m.Genres).Include(m => m.Languages)
                .Where(m => m.Languages!.Select(l => l.Name).Contains(language) && m.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByRatingAsync(CertificateRating rating)
        {
            return await _context.Movies.Include(m => m.Genres).Include(m => m.Languages)
                .Where(m => m.CertificateRating == rating && m.IsActive)
                .ToListAsync();
        }
    }
}