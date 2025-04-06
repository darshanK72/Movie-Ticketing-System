using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Persistence;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly TicketingDbContext _context;

        public BookingRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .Include(b => b.Payments)
                .Where(b => b.UserId.ToString() == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking> CreateInitialBookingAsync(string userId, string showId, List<string> seatIds, int numberOfTickets, decimal totalAmount)
        {
            var existingShowSeats = await _context.ShowSeats
                .Where(ss => ss.ShowId.ToString() == showId && 
                           seatIds.Contains(ss.SeatId.ToString()) && 
                           (ss.IsBooked || ss.BookingStatus == SeatBookingStatus.InProgress))
                .ToListAsync();

            if (existingShowSeats.Any())
            {
                throw new InvalidOperationException("One or more seats are already booked or in progress for this show");
            }

            var booking = new Booking
            {
                UserId = userId,
                ShowId = Guid.Parse(showId),
                NumberOfTickets = numberOfTickets,
                TotalAmount = totalAmount,
                Status = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                BookingDate = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            var showSeats = seatIds.Select(seatId => new ShowSeat
            {
                ShowId = Guid.Parse(showId),
                SeatId = Guid.Parse(seatId),
                BookingId = booking.Id,
                IsBooked = true,
                BookingStatus = SeatBookingStatus.InProgress,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _context.ShowSeats.AddRangeAsync(showSeats);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> ProcessPaymentAsync(Guid bookingId, string paymentMethod, string transactionId)
        {
            try
            {
                var booking = await GetByIdAsync(bookingId);
                if (booking == null || booking.Status != BookingStatus.Pending)
                    return false;

                var payment = new Payment
                {
                    BookingId = booking.Id,
                    Amount = booking.TotalAmount,
                    PaymentMethod = paymentMethod,
                    TransactionId = transactionId,
                    Status = PaymentStatus.Completed,
                    PaymentDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Payments.AddAsync(payment);

                booking.Status = BookingStatus.Confirmed;
                booking.PaymentStatus = PaymentStatus.Completed;
                booking.UpdatedAt = DateTime.UtcNow;

                foreach (var showSeat in booking.ShowSeats)
                {
                    showSeat.BookingStatus = SeatBookingStatus.Booked;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CancelExpiredBookingsAsync()
        {
            try
            {
                var expiredBookings = await _context.Bookings
                    .Include(b => b.ShowSeats)
                    .Where(b => b.Status == BookingStatus.Pending && b.ExpirationTime < DateTime.UtcNow)
                    .ToListAsync();

                foreach (var booking in expiredBookings)
                {
                    booking.Status = BookingStatus.Cancelled;
                    booking.CancellationDate = DateTime.UtcNow;
                    booking.CancellationReason = "Booking expired";
                    booking.UpdatedAt = DateTime.UtcNow;

                    foreach (var showSeat in booking.ShowSeats)
                    {
                        showSeat.IsBooked = false;
                        showSeat.BookingId = null;
                        showSeat.BookingStatus = SeatBookingStatus.Available;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CancelBookingAsync(Guid bookingId, string reason)
        {
            try
            {
                var booking = await GetByIdAsync(bookingId);
                if (booking == null)
                    return false;

                booking.Status = BookingStatus.Cancelled;
                booking.CancellationDate = DateTime.UtcNow;
                booking.CancellationReason = reason;
                booking.UpdatedAt = DateTime.UtcNow;

                foreach (var payment in booking.Payments)
                {
                    payment.Status = PaymentStatus.Refunded;
                    payment.RefundDate = DateTime.UtcNow;
                    payment.RefundReason = reason;
                    payment.UpdatedAt = DateTime.UtcNow;
                }

                foreach (var showSeat in booking.ShowSeats)
                {
                    showSeat.IsBooked = false;
                    showSeat.BookingId = null;
                    showSeat.BookingStatus = SeatBookingStatus.Available;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            try
            {
                _context.Bookings.Update(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            try
            {
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateShowSeatsAsync(List<ShowSeat> showSeats)
        {
            try
            {
                _context.ShowSeats.UpdateRange(showSeats);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}