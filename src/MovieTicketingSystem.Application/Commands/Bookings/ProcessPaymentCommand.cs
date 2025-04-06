using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class ProcessPaymentCommand : IRequest<BookingDTO>
    {
        public string? BookingId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }
} 