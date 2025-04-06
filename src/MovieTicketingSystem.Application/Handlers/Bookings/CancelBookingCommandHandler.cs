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
            return await _bookingRepository.CancelBookingAsync(request.BookingId, request.CancellationReason);
        }
    }
} 