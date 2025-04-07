using MediatR;
using MovieTicketingSystem.Application.Commands.Bookings;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Handlers.Bookings
{
    internal class ConfirmBookingCommandHandler : IRequestHandler<ConfirmBookingCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;

        public ConfirmBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            if (booking == null)
                return false;

            foreach(var seat in booking.ShowSeats!)
            {
                seat.SeatBookingStatus = SeatBookingStatus.Reserved;
            }

            booking.BookingStatus = BookingStatus.Confirmed;
            booking.UpdatedAt = DateTime.UtcNow;

            return await _bookingRepository.UpdateBookingAsync(booking);
        }
    }
} 