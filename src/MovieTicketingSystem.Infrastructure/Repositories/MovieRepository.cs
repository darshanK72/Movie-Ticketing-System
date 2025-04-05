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
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(string id)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.Id.ToString() == id);
        }

        public async Task<bool> CreateMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task<bool> DeleteMovieAsync(string id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Movie>> GetActiveMoviesAsync()
        {
            return await _context.Movies
                .Where(m => m.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre)
        {
            return await _context.Movies
                .Where(m => m.Genre == genre && m.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByLanguageAsync(string language)
        {
            return await _context.Movies
                .Where(m => m.Language == language && m.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetMoviesByRatingAsync(MovieRating rating)
        {
            return await _context.Movies
                .Where(m => m.Rating == rating && m.IsActive)
                .ToListAsync();
        }
    }
}