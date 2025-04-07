using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Infrastructure.Persistence;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class ShowTimingRepository : IShowTimingRepository
    {
        private readonly TicketingDbContext _context;

        public ShowTimingRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<ShowTiming?> GetShowTimingByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return null;

            return await _context.ShowTimings
                .Include(st => st.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(st => st.Show)
                    .ThenInclude(s => s!.CinemaHall)
                        .ThenInclude(ch => ch!.Theater)
                .Include(st => st.ShowManager)
                .Include(st => st.Bookings)
                .Include(st => st.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .FirstOrDefaultAsync(st => st.Id == guidId);
        }

        public async Task<IEnumerable<ShowTiming>> GetShowTimingsByShowIdAsync(string showId)
        {
            if (!Guid.TryParse(showId, out Guid guidShowId))
                return Enumerable.Empty<ShowTiming>();

            return await _context.ShowTimings
                .Include(st => st.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(st => st.Show)
                    .ThenInclude(s => s!.CinemaHall)
                        .ThenInclude(ch => ch!.Theater)
                .Include(st => st.ShowManager)
                .Include(st => st.Bookings)
                .Include(st => st.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .Where(st => st.ShowId == guidShowId)
                .OrderBy(st => st.Date)
                .ThenBy(st => st.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShowTiming>> GetAllShowTimingsAsync()
        {
            return await _context.ShowTimings
                .Include(st => st.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(st => st.Show)
                    .ThenInclude(s => s!.CinemaHall)
                        .ThenInclude(ch => ch!.Theater)
                .Include(st => st.ShowManager)
                .Include(st => st.Bookings)
                .Include(st => st.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .OrderBy(st => st.Date)
                .ThenBy(st => st.StartTime)
                .ToListAsync();
        }

        public async Task<bool> CreateShowTimingAsync(ShowTiming showTiming)
        {
            try
            {
                await _context.ShowTimings.AddAsync(showTiming);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateShowTimingAsync(ShowTiming showTiming)
        {
            try
            {
                showTiming.UpdatedAt = DateTime.UtcNow;
                _context.ShowTimings.Update(showTiming);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteShowTimingAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var showTiming = await _context.ShowTimings.FindAsync(guidId);
                if (showTiming == null)
                    return false;

                _context.Remove(showTiming);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActivateShowTimingAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var showTiming = await _context.ShowTimings.FindAsync(guidId);
                if (showTiming == null)
                    return false;

                showTiming.IsActive = true;
                showTiming.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateShowTimingAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var showTiming = await _context.ShowTimings.FindAsync(guidId);
                if (showTiming == null)
                    return false;

                showTiming.IsActive = false;
                showTiming.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 