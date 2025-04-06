using MediatR;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class CancelBookingCommand : IRequest<bool>
    {
        public string BookingId { get; set; } = string.Empty;
        public string CancellationReason { get; set; } = string.Empty;
    }
} 