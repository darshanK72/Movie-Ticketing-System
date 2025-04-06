using MediatR;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class ConfirmBookingCommand : IRequest<bool>
    {
        public string BookingId { get; set; } = string.Empty;
    }
} 