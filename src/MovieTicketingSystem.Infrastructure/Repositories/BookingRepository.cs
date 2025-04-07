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

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .Include(b => b.Payments)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.ShowSeats!)
                    .ThenInclude(ss => ss.Seat)
                .Include(b => b.ShowTiming)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id.ToString() == id);
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

        public async Task<Booking> CreateInitialBookingAsync(string userId, string showTimingId, List<string> seatIds)
        {
            var showTiming = await _context.ShowTimings
                .FirstOrDefaultAsync(st => st.Id.ToString() == showTimingId);

            if (showTiming == null)
            {
                throw new InvalidOperationException("Show timing not found");
            }

            var showSeatsa = await _context.ShowSeats
                .Include(ss => ss.Seat)
                .Where(ss => ss.ShowTimingId.ToString() == showTimingId)
                .ToListAsync();

            var showSeats = await _context.ShowSeats
                .Include(ss => ss.Seat)
                .Where(ss => ss.ShowTimingId.ToString() == showTimingId && 
                           seatIds.Contains(ss.Id.ToString()))
                .ToListAsync();

            if (showSeats.Count != seatIds.Count)
            {
                throw new InvalidOperationException("One or more seats not found for this show timing");
            }

            var unavailableSeats = showSeats
                .Where(ss => ss.IsBooked || ss.SeatBookingStatus == SeatBookingStatus.InProgress)
                .ToList();

            if (unavailableSeats.Any())
            {
                throw new InvalidOperationException("One or more seats are already booked or in progress for this show timing");
            }

            int numberOfTickets = showSeats.Count();
            decimal calculatedTotalAmount = showSeats.Sum(ss => showTiming.BasePrice * ss.Seat!.PriceMultiplier);

            var booking = new Booking
            {
                UserId = userId,
                ShowTimingId = Guid.Parse(showTimingId),
                NumberOfTickets = numberOfTickets,
                TotalAmount = calculatedTotalAmount,
                BookingStatus = BookingStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                BookingDate = DateTime.UtcNow,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            foreach (var showSeat in showSeats)
            {
                showSeat.BookingId = booking.Id;
                showSeat.IsBooked = true;
                showSeat.SeatBookingStatus = SeatBookingStatus.InProgress;
                showSeat.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<IEnumerable<ShowSeat>> GetShowSeatsByBookingId(string bookingId)
        {
            return await _context.ShowSeats.Where(ss => ss.BookingId.ToString() == bookingId).ToListAsync();
        }
        public async Task<bool> ProcessPaymentAsync(string bookingId, string paymentMethod, string transactionId)
        {
            try
            {
                var booking = await GetByIdAsync(bookingId);
                if (booking == null)
                    return false;

                var payment = new Payment
                {
                    BookingId = booking.Id,
                    Amount = booking.TotalAmount,
                    PaymentMethod = paymentMethod,
                    TransactionId = transactionId,
                    PaymentStatus = PaymentStatus.Completed,
                    PaymentDate = DateTime.UtcNow,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                booking.PaymentStatus = PaymentStatus.Completed;
                booking.BookingStatus = BookingStatus.Confirmed;
                booking.UpdatedAt = DateTime.UtcNow;

                foreach (var showSeat in booking.ShowSeats!)
                {
                    showSeat.SeatBookingStatus = SeatBookingStatus.Booked;
                    showSeat.UpdatedAt = DateTime.UtcNow;
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
                    .Where(b => b.BookingStatus == BookingStatus.Pending && 
                               b.ExpirationTime < DateTime.UtcNow)
                    .ToListAsync();

                foreach (var booking in expiredBookings)
                {
                    booking.BookingStatus = BookingStatus.Cancelled;
                    booking.CancellationDate = DateTime.UtcNow;
                    booking.CancellationReason = "Booking expired";
                    booking.UpdatedAt = DateTime.UtcNow;

                    foreach (var showSeat in booking.ShowSeats!)
                    {
                        showSeat.IsBooked = false;
                        showSeat.BookingId = null;
                        showSeat.SeatBookingStatus = SeatBookingStatus.Available;
                    }

                    booking.ShowSeats = null;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CancelBookingAsync(string bookingId, string reason)
        {
            try
            {
                var booking = await GetByIdAsync(bookingId);
                if (booking == null)
                    return false;

                booking.BookingStatus = BookingStatus.Cancelled;
                booking.CancellationDate = DateTime.UtcNow;
                booking.CancellationReason = reason;
                booking.UpdatedAt = DateTime.UtcNow;
                booking.PaymentStatus = PaymentStatus.Refunded;

                foreach (var payment in booking.Payments!)
                {
                    payment.PaymentStatus = PaymentStatus.Refunded;
                    payment.RefundDate = DateTime.UtcNow;
                    payment.RefundReason = reason;
                    payment.UpdatedAt = DateTime.UtcNow;
                }

                foreach (var showSeat in booking.ShowSeats!)
                {
                    showSeat.IsBooked = false;
                    showSeat.BookingId = null;
                    showSeat.SeatBookingStatus = SeatBookingStatus.Available;
                    showSeat.UpdatedAt = DateTime.UtcNow;
                }

                booking.ShowSeats = null;
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
                booking.UpdatedAt = DateTime.UtcNow;
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
                payment.UpdatedAt = DateTime.UtcNow;
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
                foreach (var showSeat in showSeats)
                {
                    showSeat.UpdatedAt = DateTime.UtcNow;
                }
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