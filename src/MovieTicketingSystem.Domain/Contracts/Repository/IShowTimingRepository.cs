using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface IShowTimingRepository
    {
        Task<ShowTiming?> GetShowTimingByIdAsync(string id);
        Task<IEnumerable<ShowTiming>> GetShowTimingsByShowIdAsync(string showId);
        Task<IEnumerable<ShowTiming>> GetAllShowTimingsAsync();
        Task<bool> CreateShowTimingAsync(ShowTiming showTiming);
        Task<bool> UpdateShowTimingAsync(ShowTiming showTiming);
        Task<bool> DeleteShowTimingAsync(string id);
        Task<bool> ActivateShowTimingAsync(string id);
        Task<bool> DeactivateShowTimingAsync(string id);
    }
} 