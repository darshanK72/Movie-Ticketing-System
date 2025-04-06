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
using MovieTicketingSystem.Application.Services;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly TicketingDbContext _context;

        public TheaterRepository(TicketingDbContext context)
        {
            _context = context;
        }
       
        public async Task<Theater?> GetTheaterByIdAsync(string id)
        {
            return await _context.Theaters
                .Include(t => t.Address)
                .Include(t => t.CinemaHalls)
                .FirstOrDefaultAsync(t => t.Id.ToString() == id);
        }

        public async Task<IEnumerable<Theater>> GetAllTheatersAsync()
        {
            return await _context.Theaters
                .Include(t => t.Address)
                .Include(t => t.CinemaHalls)
                .ToListAsync();
        }

        public async Task<IEnumerable<Theater>> GetTheatersByCityAsync(string city)
        {
            return await _context.Theaters
                .Include(t => t.Address)
                .Where(t => t.Address!.City!.ToLower() == city.ToLower())
                .ToListAsync();
        }

        public async Task<PagedResult<Theater>> GetTheatersPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var query = _context.Theaters
                .Include(t => t.Address)
                .Include(t => t.CinemaHalls)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(t => 
                    t.Name!.ToLower().Contains(searchTerm) || 
                    t.Description!.ToLower().Contains(searchTerm) ||
                    t.Email!.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Theater>(items, totalCount, pageSize, pageNumber);
        }

        public async Task<bool> CreateTheaterAsync(Theater theater, Address address)
        {
            address.CreatedAt = DateTime.UtcNow;
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();

            theater.AddressId = address.Id;
            theater.CreatedAt = DateTime.UtcNow;
            theater.IsActive = true;

            await _context.Theaters.AddAsync(theater);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTheaterAsync(Theater theater,Address address)
        {
            var existingTheater = await _context.Theaters.FindAsync(theater.Id);

            if (existingTheater == null)
                throw new NotFoundException("Theater", theater.Id.ToString());

            var existingAddress = await _context.Addresses.FindAsync(address.Id);

            if (existingAddress == null)
                throw new NotFoundException("Address", address.Id.ToString());

            existingTheater.Name = theater.Name;
            existingTheater.Description = theater.Description;
            existingTheater.ContactNumber = theater.ContactNumber;
            existingTheater.Email = theater.Email;
            existingTheater.Website = theater.Website;
            existingTheater.UpdatedAt = DateTime.UtcNow;

            existingAddress.State = address.State;
            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.Details = address.Details;
            existingAddress.PostalCode = address.PostalCode;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTheaterAsync(string id)
        {
            var theater = await _context.Theaters.Include(t => t.CinemaHalls).FirstOrDefaultAsync(t => t.Id == Guid.Parse(id));

            if (theater == null)
                return false;

            foreach (var hall in theater!.CinemaHalls!)
            {
                await DeleteCinemaHallAsync(hall.Id.ToString());
            }
         
            _context.Theaters.Remove(theater);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateTheaterAsync(string id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
                return false;

            theater.IsActive = true;
            theater.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateTheaterAsync(string id)
        {
            var theater = await _context.Theaters.FindAsync(id);
            if (theater == null)
                return false;

            theater.IsActive = false;
            theater.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Address> CreateAddressAsync(Address address)
        {
            address.CreatedAt = DateTime.UtcNow;
            
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            
            return address;
        }

        public async Task<Address?> GetAddressByIdAsync(string id)
        {
            return await _context.Addresses.FindAsync(Guid.Parse(id));
        }

        public async Task<Address> UpdateAddressAsync(Address address)
        {
            var existingAddress = await _context.Addresses.FindAsync(address.Id);
            if (existingAddress == null)
                throw new NotFoundException("Address", address.Id.ToString());

            existingAddress.Details = address.Details;
            existingAddress.Street = address.Street;
            existingAddress.City = address.City;
            existingAddress.State = address.State;
            existingAddress.PostalCode = address.PostalCode;
            existingAddress.Country = address.Country;
            existingAddress.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingAddress;
        }

        public async Task<CinemaHall?> GetCinemaHallByIdAsync(string id)
        {
            return await _context.CinemaHalls
                .Include(ch => ch.Seats)
                .FirstOrDefaultAsync(ch => ch.Id.ToString() == id);
        }
        public async Task<IEnumerable<CinemaHall>> GetCinemaHallsByTheaterIdAsync(string theaterId)
        {
            return await _context.CinemaHalls
                .Include(ch => ch.Seats)
                .Where(ch => ch.TheaterId.ToString() == theaterId)
                .ToListAsync();
        }

        public async Task<CinemaHall> CreateCinemaHallAsync(CinemaHall cinemaHall)
        {
            cinemaHall.CreatedAt = DateTime.UtcNow;
            cinemaHall.IsActive = true;
            
            await _context.CinemaHalls.AddAsync(cinemaHall);
            await _context.SaveChangesAsync();
            
            return cinemaHall;
        }

        public async Task<bool> UpdateCinemaHallAsync(CinemaHall cinemaHall)
        {
            var existingCinemaHall = await _context.CinemaHalls.FindAsync(cinemaHall.Id);
            if (existingCinemaHall == null)
                throw new NotFoundException("CinemaHall", cinemaHall.Id.ToString());

            bool isSeatingConfigChanged = cinemaHall.NumberOfRows != existingCinemaHall.NumberOfRows ||
                                        cinemaHall.SeatsPerRow != existingCinemaHall.SeatsPerRow ||
                                        cinemaHall.TotalSeats != existingCinemaHall.TotalSeats;

            existingCinemaHall.Name = cinemaHall.Name;
            existingCinemaHall.Description = cinemaHall.Description;
            existingCinemaHall.TotalSeats = cinemaHall.TotalSeats;
            existingCinemaHall.NumberOfRows = cinemaHall.NumberOfRows;
            existingCinemaHall.SeatsPerRow = cinemaHall.SeatsPerRow;
            existingCinemaHall.Has3D = cinemaHall.Has3D;
            existingCinemaHall.HasDolby = cinemaHall.HasDolby;
            existingCinemaHall.UpdatedAt = DateTime.UtcNow;

            if (isSeatingConfigChanged)
            {
                var existingSeats = await _context.Seats
                    .Where(s => s.CinemaHallId == existingCinemaHall.Id)
                    .ToListAsync();
                _context.Seats.RemoveRange(existingSeats);
                await _context.SaveChangesAsync();

                var seatGenerationService = new SeatGenerationService();
                var newSeats = seatGenerationService.GenerateSeatsForCinemaHall(existingCinemaHall);
                await _context.Seats.AddRangeAsync(newSeats);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCinemaHallAsync(string id)
        {
            var cinemaHall = await _context.CinemaHalls.FindAsync(Guid.Parse(id));

            var cineHallSeats = await _context.Seats.Where(seat => seat.CinemaHallId.ToString() == id).ToListAsync();

            if(cineHallSeats != null)
            {
                _context.Seats.RemoveRange(cineHallSeats);
            }

            if (cinemaHall == null)
                return false;

            _context.CinemaHalls.Remove(cinemaHall);
            await _context.SaveChangesAsync();
            return true;
        }
       
        public async Task<bool> CreateCinemaHallWithSeatsAsync(CinemaHall cinemaHall, IEnumerable<Seat> seats)
        {
            cinemaHall.CreatedAt = DateTime.UtcNow;
            cinemaHall.IsActive = true;
            await _context.CinemaHalls.AddAsync(cinemaHall);
            await _context.SaveChangesAsync();

            foreach (var seat in seats)
            {
                seat.CinemaHallId = cinemaHall.Id;
                seat.TheaterId = cinemaHall.TheaterId;
                seat.CreatedAt = DateTime.UtcNow;
                seat.IsActive = true;
            }

            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();

            return true;
        }
        
        public async Task<Seat?> GetSeatByIdAsync(string id)
        {
            return await _context.Seats.FindAsync(id);
        }

        public async Task<IEnumerable<Seat>> GetSeatsByCinemaHallIdAsync(string cinemaHallId)
        {
            return await _context.Seats
                .Where(s => s.CinemaHallId.ToString() == cinemaHallId)
                .ToListAsync();
        }

        public async Task<Seat> CreateSeatAsync(Seat seat)
        {
            seat.CreatedAt = DateTime.UtcNow;
            seat.IsActive = true;
            
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
            
            return seat;
        }

        public async Task<IEnumerable<Seat>> CreateSeatsAsync(IEnumerable<Seat> seats)
        {
            foreach (var seat in seats)
            {
                seat.CreatedAt = DateTime.UtcNow;
                seat.IsActive = true;
            }
            
            await _context.Seats.AddRangeAsync(seats);
            await _context.SaveChangesAsync();
            
            return seats;
        }

        public async Task<Seat> UpdateSeatAsync(Seat seat)
        {
            var existingSeat = await _context.Seats.FindAsync(seat.Id);
            if (existingSeat == null)
                throw new NotFoundException("Seat", seat.Id.ToString());

            existingSeat.SeatNumber = seat.SeatNumber;
            existingSeat.RowNumber = seat.RowNumber;
            existingSeat.ColumnNumber = seat.ColumnNumber;
            existingSeat.Type = seat.Type;
            existingSeat.PriceMultiplier = seat.PriceMultiplier;
            existingSeat.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingSeat;
        }

        public async Task<bool> DeleteSeatAsync(string id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
                return false;

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return true;
        }

    }
} 