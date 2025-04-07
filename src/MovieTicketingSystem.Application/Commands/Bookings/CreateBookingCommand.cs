using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class CreateBookingCommand : IRequest<string>
    {
        public string? UserId { get; set; } = string.Empty;
        public string? ShowTimingId { get; set; }
        public List<string> SeatIds { get; set; } = new();
    }
} 