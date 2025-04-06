using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Bookings
{
    public class GetBookingByIdQuery : IRequest<BookingDTO?>
    {
        public string BookingId { get; set; } = string.Empty;

        public GetBookingByIdQuery(string id){
            BookingId = id;
        }
    }
} 