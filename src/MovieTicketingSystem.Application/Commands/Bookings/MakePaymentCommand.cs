using MediatR;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class MakePaymentCommand : IRequest<bool>
    {
        public string BookingId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }
} 