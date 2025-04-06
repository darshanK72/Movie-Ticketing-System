using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task<List<Booking>> GetUserBookingsAsync(string userId);
        Task<Booking> CreateInitialBookingAsync(string userId, string showId, List<string> seatIds, int numberOfTickets, decimal totalAmount);
        Task<bool> ProcessPaymentAsync(Guid bookingId, string paymentMethod, string transactionId);
        Task<bool> CancelExpiredBookingsAsync();
        Task<bool> CancelBookingAsync(Guid bookingId, string reason);
        Task<bool> UpdateBookingAsync(Booking booking);
        Task<bool> UpdatePaymentAsync(Payment payment);
        Task<bool> UpdateShowSeatsAsync(List<ShowSeat> showSeats);
    }
} 