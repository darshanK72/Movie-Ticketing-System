using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface IShowSeatRepository
    {
        Task<ShowSeat?> GetShowSeatByIdAsync(string id);
        Task<IEnumerable<ShowSeat>> GetShowSeatsByShowTimingIdAsync(string showTimingId);
        Task<IEnumerable<ShowSeat>> GetAllShowSeatsAsync();
        Task<bool> CreateShowSeatAsync(ShowSeat showSeat);
        Task<bool> UpdateShowSeatAsync(ShowSeat showSeat);
        Task<bool> DeleteShowSeatAsync(string id);
        Task<bool> ActivateShowSeatAsync(string id);
        Task<bool> DeactivateShowSeatAsync(string id);
        Task<IEnumerable<Seat>> GetSeatsByCinemaHallAsync(Guid cinemaHallId);
        Task<bool> UpdateShowSeatStatusAsync(string showSeatId, SeatBookingStatus status);
    }
} 