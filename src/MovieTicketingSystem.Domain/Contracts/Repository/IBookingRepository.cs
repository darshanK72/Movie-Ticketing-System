using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetByIdAsync(string id);
        Task<List<Booking>> GetUserBookingsAsync(string userId);
        Task<Booking> CreateInitialBookingAsync(string userId, string showId, List<string> seatIds);
        Task<bool> ProcessPaymentAsync(string bookingId, string paymentMethod, string transactionId);
        Task<bool> CancelExpiredBookingsAsync();
        Task<bool> CancelBookingAsync(string bookingId, string reason);
        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> UpdatePaymentAsync(Payment payment);
        Task<bool> UpdateShowSeatsAsync(List<ShowSeat> showSeats);
    }
} 