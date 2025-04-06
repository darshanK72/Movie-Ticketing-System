using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Bookings
{
    public class GetUserBookingsQuery : IRequest<List<BookingDTO>>
    {
        public string UserId { get; set; } = string.Empty;

        public GetUserBookingsQuery(string userId){
            UserId = userId;
        }
    }
} 