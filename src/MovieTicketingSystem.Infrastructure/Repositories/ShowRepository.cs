using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Infrastructure.Persistence;
using MovieTicketingSystem.Application.Common;
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
                .Include(s => s.ShowManager)
                .Include(s => s.ShowSeats)
                .FirstOrDefaultAsync(s => s.Id == guidId);
        }

        public async Task<IEnumerable<Show>> GetAllShowsAsync()
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowManager)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsByMovieAsync(string movieId)
        {
            if (!Guid.TryParse(movieId, out Guid guidMovieId))
                return Enumerable.Empty<Show>();

            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .Include(s => s.ShowManager)
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
                .Include(s => s.ShowManager)
                .Where(s => s.CinemaHall!.TheaterId == guidTheaterId)
                .ToListAsync();
        }

        public async Task<PagedResult<Show>> GetShowsPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var query = _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowManager)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(s => 
                    s.Movie!.Title!.ToLower().Contains(searchTerm) || 
                    s.Movie!.Description!.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Show>(items, totalCount, pageSize, pageNumber);
        }

        public async Task<bool> CreateShowAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();

            var seats = await _context.Seats
                .Where(s => s.CinemaHallId == show.CinemaHallId && s.IsActive)
                .ToListAsync();

            if (seats == null) return false;

            var showSeats = seats.Select(seat => new ShowSeat
            {
                ShowId = show.Id,
                SeatId = seat.Id,
                IsBooked = false,
                BookingStatus = SeatBookingStatus.Available,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _context.ShowSeats.AddRangeAsync(showSeats);
            await _context.SaveChangesAsync();

            show.TotalSeats = seats.Count;
            show.AvailableSeats = seats.Count;
            await _context.SaveChangesAsync();

            return true ;
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
            try{
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var show = await _context.Shows.FindAsync(guidId);
                if (show == null)
                    return false;

                _context.Remove(show);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception){
                throw;
            }
        }

        public async Task<bool> ActivateShowAsync(string id)
        {
            try{
                if (!Guid.TryParse(id, out Guid guidId))
                    return false;

                var show = await _context.Shows.FindAsync(guidId);
                if (show == null)
                    return false;

                show.IsActive = true;
                show.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception){
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
                .Include(s => s.ShowManager)
                .Include(s => s.ShowSeats)
                .FirstOrDefaultAsync(s => s.Id == guidShowId);
        }

        public async Task<IEnumerable<Show>> GetUpcomingShowsAsync()
        {
            var currentDate = DateTime.UtcNow.Date;
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowManager)
                .Where(s => s.Date >= currentDate && s.IsActive)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetTodayShowsAsync()
        {
            var today = DateTime.Today;
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Include(s => s.ShowManager)
                .Where(s => s.Date.Date == today && s.IsActive)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsByMovieIdAsync(Guid movieId)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall).ThenInclude(ch => ch!.Theater)
                .Where(s => s.MovieId == movieId && s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetShowsByCinemaHallIdAsync(Guid cinemaHallId)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .Where(s => s.CinemaHallId == cinemaHallId && s.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShowSeat>> GetShowSeatsAsync(Guid showId)
        {
            return await _context.ShowSeats
                .Include(ss => ss.Seat)
                .Where(ss => ss.ShowId == showId && ss.IsActive)
                .OrderBy(ss => ss.Seat!.RowNumber)
                .ThenBy(ss => ss.Seat!.ColumnNumber)
                .ToListAsync();
        }

        public async Task<ShowSeat> UpdateShowSeatStatusAsync(Guid showId, int seatNumber, SeatBookingStatus status)
        {
            var showSeat = await _context.ShowSeats
                .Include(ss => ss.Seat)
                .FirstOrDefaultAsync(ss => ss.ShowId == showId && ss.Seat!.SeatNumber == seatNumber.ToString());

            if (showSeat == null)
                throw new KeyNotFoundException($"Seat {seatNumber} not found for show {showId}");

            showSeat.BookingStatus = status;
            showSeat.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return showSeat;
        }

        public async Task<Show?> GetByIdAsync(Guid id)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .Include(s => s.ShowManager)
                .Include(s => s.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Show>> GetShowsByMovieAsync(Guid movieId)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .Include(s => s.ShowSeats)
                .Where(s => s.MovieId == movieId && s.IsActive)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<Show>> GetShowsByTheaterAsync(Guid theaterId)
        {
            return await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.CinemaHall)
                .Include(s => s.ShowSeats)
                .Where(s => s.CinemaHall.TheaterId == theaterId && s.IsActive)
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }
    }
} 