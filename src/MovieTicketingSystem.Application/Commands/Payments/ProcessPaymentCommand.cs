using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Commands.Payments
{
    public class ProcessPaymentCommand : IRequest<PaymentDTO>
    {
        public string? BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Dictionary<string, string> PaymentDetails { get; set; } = new Dictionary<string, string>();
    }
} 