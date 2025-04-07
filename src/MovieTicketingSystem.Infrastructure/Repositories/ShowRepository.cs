using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Infrastructure.Persistence;
using MovieTicketingSystem.Domain.Exceptions;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Application.Repositories;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly TicketingDbContext _context;

        public ShowRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Show?> GetShowByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return null;

            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowManager)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowSeats)
                        .ThenInclude(ss => ss.Seat)
                .FirstOrDefaultAsync(s => s.Id == guidId);
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync()
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowManager)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowSeats)
                        .ThenInclude(ss => ss.Seat)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsByMovieAsync(string movieId)
        {
            if (!Guid.TryParse(movieId, out Guid guidMovieId))
                return Enumerable.Empty<Show>();

            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowManager)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowSeats)
                        .ThenInclude(ss => ss.Seat)
                .Where(s => s.MovieId == guidMovieId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsByTheaterAsync(string theaterId)
        {
            if (!Guid.TryParse(theaterId, out Guid guidTheaterId))
                return Enumerable.Empty<Show>();

            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowManager)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowSeats)
                        .ThenInclude(ss => ss.Seat)
                .Where(s => s.CinemaHall!.TheaterId == guidTheaterId)
                .ToListAsync();
        }

        
        public async Task<bool> CreateShowAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateShowAsync(Show show)
        {
            try
            {
                show.UpdatedAt = DateTime.UtcNow;
                _context.Shows.Update(show);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteShowAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var show = await _context.Shows.FindAsync(guidId);
                if (show == null)
                    return false;

                _context.Remove(show);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ActivateShowAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var show = await _context.Shows.FindAsync(guidId);
                if (show == null)
                    return false;

                show.IsActive = true;
                show.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeactivateShowAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return false;

            var show = await _context.Shows.FindAsync(guidId);
            if (show == null)
                return false;

            show.IsActive = false;
            show.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Show?> GetShowWithSeatsAsync(string showId)
        {
            if (!Guid.TryParse(showId, out Guid guidShowId))
                return null;

            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowManager)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowSeats)
                        .ThenInclude(ss => ss.Seat)
                .FirstOrDefaultAsync(s => s.Id == guidShowId);
        }

        public async Task<IEnumerable<Show>> GetUpcomingShowsAsync()
        {
            var currentDate = DateTime.UtcNow.Date;
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowManager)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowSeats)
                        .ThenInclude(ss => ss.Seat)
                .Where(s => s.ShowTimings!.Any(st => st.Date >= currentDate && st.IsActive))
                .OrderBy(s => s.ShowTimings!.Min(st => st.Date))
                .ThenBy(s => s.ShowTimings!.Min(st => st.StartTime))
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetTodayShowsAsync()
        {
            var today = DateTime.Today;
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowManager)
                .Include(s => s.ShowTimings)
                    .ThenInclude(st => st.ShowSeats)
                        .ThenInclude(ss => ss.Seat)
                .Where(s => s.ShowTimings!.Any(st => st.Date.Date == today && st.IsActive))
                .OrderBy(s => s.ShowTimings!.Min(st => st.StartTime))
                .ToListAsync();
        }

        public async Task<IEnumerable<ShowTiming>> GetShowTimingsByShowIdAsync(Guid showId)
        {
            return await _context.ShowTimings
                .Include(st => st.ShowManager)
                .Where(st => st.ShowId == showId && st.IsActive)
                .OrderBy(st => st.Date)
                .ThenBy(st => st.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShowSeat>> GetShowSeatsByShowTimingIdAsync(Guid showTimingId)
        {
            return await _context.ShowSeats
                .Include(ss => ss.Seat)
                .Where(ss => ss.ShowTimingId == showTimingId && ss.IsActive)
                .OrderBy(ss => ss.Seat.RowNumber)
                .ThenBy(ss => ss.Seat.ColumnNumber)
                .ToListAsync();
        }
    }
}