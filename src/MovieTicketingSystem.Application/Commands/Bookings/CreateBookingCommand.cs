using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class CreateBookingCommand : IRequest<string>
    {
        public string? UserId { get; set; } = string.Empty;
        public string? ShowId { get; set; }
        public List<string> SeatIds { get; set; } = new();
        public int NumberOfTickets { get; set; }
        public decimal TotalAmount { get; set; }
    }
} 