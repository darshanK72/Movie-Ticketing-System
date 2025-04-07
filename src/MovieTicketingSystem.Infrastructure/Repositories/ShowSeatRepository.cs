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
    public class ShowSeatRepository : IShowSeatRepository
    {
        private readonly TicketingDbContext _context;

        public ShowSeatRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<ShowSeat?> GetShowSeatByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId))
                return null;

            return await _context.ShowSeats
                .Include(ss => ss.ShowTiming)
                    .ThenInclude(st => st!.Show)
                        .ThenInclude(s => s!.Movie)
                .Include(ss => ss.ShowTiming)
                    .ThenInclude(st => st!.Show)
                        .ThenInclude(s => s!.CinemaHall)
                            .ThenInclude(ch => ch!.Theater)
                .Include(ss => ss.Seat)
                .FirstOrDefaultAsync(ss => ss.Id == guidId);
        }

        public async Task<IEnumerable<ShowSeat>> GetShowSeatsByShowTimingIdAsync(string showTimingId)
        {
            if (!Guid.TryParse(showTimingId, out Guid guidShowTimingId))
                return Enumerable.Empty<ShowSeat>();

            return await _context.ShowSeats
                .Include(ss => ss.ShowTiming)
                    .ThenInclude(st => st!.Show)
                        .ThenInclude(s => s!.Movie)
                .Include(ss => ss.ShowTiming)
                    .ThenInclude(st => st!.Show)
                        .ThenInclude(s => s!.CinemaHall)
                            .ThenInclude(ch => ch!.Theater)
                .Include(ss => ss.Seat)
                .Where(ss => ss.ShowTimingId == guidShowTimingId)
                .OrderBy(ss => ss.Seat!.RowNumber)
                .ThenBy(ss => ss.Seat!.ColumnNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShowSeat>> GetAllShowSeatsAsync()
        {
            return await _context.ShowSeats
                .Include(ss => ss.ShowTiming)
                    .ThenInclude(st => st!.Show)
                        .ThenInclude(s => s!.Movie)
                .Include(ss => ss.ShowTiming)
                    .ThenInclude(st => st!.Show)
                        .ThenInclude(s => s!.CinemaHall)
                            .ThenInclude(ch => ch!.Theater)
                .Include(ss => ss.Seat)
                .OrderBy(ss => ss.Seat!.RowNumber)
                .ThenBy(ss => ss.Seat!.ColumnNumber)
                .ToListAsync();
        }

        public async Task<bool> CreateShowSeatAsync(ShowSeat showSeat)
        {
            try
            {
                await _context.ShowSeats.AddAsync(showSeat);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateShowSeatAsync(ShowSeat showSeat)
        {
            try
            {
                showSeat.UpdatedAt = DateTime.UtcNow;
                _context.ShowSeats.Update(showSeat);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteShowSeatAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var showSeat = await _context.ShowSeats.FindAsync(guidId);
                if (showSeat == null)
                    return false;

                _context.Remove(showSeat);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActivateShowSeatAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var showSeat = await _context.ShowSeats.FindAsync(guidId);
                if (showSeat == null)
                    return false;

                showSeat.IsActive = true;
                showSeat.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateShowSeatAsync(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var showSeat = await _context.ShowSeats.FindAsync(guidId);
                if (showSeat == null)
                    return false;

                showSeat.IsActive = false;
                showSeat.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Seat>> GetSeatsByCinemaHallAsync(Guid cinemaHallId)
        {
            return await _context.Seats
                .Where(s => s.CinemaHallId == cinemaHallId && s.IsActive)
                .OrderBy(s => s.RowNumber)
                .ThenBy(s => s.ColumnNumber)
                .ToListAsync();
        }

        public async Task<bool> UpdateShowSeatStatusAsync(string showSeatId, SeatBookingStatus status)
        {
            try
            {
                if (!Guid.TryParse(showSeatId, out Guid guidId))
                    return false;

                var showSeat = await _context.ShowSeats.FindAsync(guidId);
                if (showSeat == null)
                    return false;

                showSeat.SeatBookingStatus = status;
                showSeat.IsBooked = status == SeatBookingStatus.Booked;
                showSeat.UpdatedAt = DateTime.UtcNow;
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