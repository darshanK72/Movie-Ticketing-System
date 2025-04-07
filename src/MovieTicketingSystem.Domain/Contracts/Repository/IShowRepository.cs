using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Application.Common;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface IShowRepository
    {
        Task<Show?> GetShowByIdAsync(string id);
        Task<IEnumerable<Show>> GetAllShowsAsync();
        Task<IEnumerable<Show>> GetShowsByMovieAsync(string movieId);
        Task<IEnumerable<Show>> GetShowsByTheaterAsync(string theaterId);
        Task<PagedResult<Show>> GetShowsPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<bool> CreateShowAsync(Show show);
        Task<bool> UpdateShowAsync(Show show);
        Task<bool> DeleteShowAsync(string id);
        Task<bool> ActivateShowAsync(string id);
        Task<bool> DeactivateShowAsync(string id);
        Task<Show?> GetShowWithSeatsAsync(string showId);
        Task<IEnumerable<Show>> GetUpcomingShowsAsync();
        Task<IEnumerable<Show>> GetTodayShowsAsync();
        Task<IEnumerable<ShowTiming>> GetShowTimingsByShowIdAsync(Guid showId);
        Task<IEnumerable<ShowSeat>> GetShowSeatsByShowTimingIdAsync(Guid showTimingId);
    }
} 