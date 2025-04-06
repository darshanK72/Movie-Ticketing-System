using MediatR;
using MovieTicketingSystem.Application.Repositories;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;

        public CancelBookingCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bookingId = Guid.Parse(request.BookingId);
                return await _bookingRepository.CancelBookingAsync(bookingId, request.CancellationReason);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
} 