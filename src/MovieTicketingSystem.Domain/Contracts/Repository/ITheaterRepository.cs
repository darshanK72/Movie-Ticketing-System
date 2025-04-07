using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface ITheaterRepository
    {
        Task<Theater?> GetTheaterByIdAsync(string id);
        Task<IEnumerable<Theater>> GetAllTheatersAsync();
        Task<IEnumerable<Theater>> GetTheatersByCityAsync(string city);
        Task<bool> CreateTheaterAsync(Theater theater, Address address);
        Task<bool> UpdateTheaterAsync(Theater theater,Address address);
        Task<bool> DeleteTheaterAsync(string id);
        Task<bool> ActivateTheaterAsync(string id);
        Task<bool> DeactivateTheaterAsync(string id);
        Task<Address> CreateAddressAsync(Address address);
        Task<Address?> GetAddressByIdAsync(string id);
        Task<Address> UpdateAddressAsync(Address address);
        Task<CinemaHall?> GetCinemaHallByIdAsync(string id);
        Task<IEnumerable<CinemaHall>> GetCinemaHallsByTheaterIdAsync(string theaterId);
        Task<CinemaHall> CreateCinemaHallAsync(CinemaHall cinemaHall);
        Task<bool> UpdateCinemaHallAsync(CinemaHall cinemaHall);
        Task<bool> DeleteCinemaHallAsync(string id);
        Task<bool> CreateCinemaHallWithSeatsAsync(CinemaHall cinemaHall, IEnumerable<Seat> seats);
        Task<Seat?> GetSeatByIdAsync(string id);
        Task<IEnumerable<Seat>> GetSeatsByCinemaHallIdAsync(string cinemaHallId);
        Task<Seat> CreateSeatAsync(Seat seat);
        Task<IEnumerable<Seat>> CreateSeatsAsync(IEnumerable<Seat> seats);
        Task<Seat> UpdateSeatAsync(Seat seat);
        Task<bool> DeleteSeatAsync(string id);
    }
} 