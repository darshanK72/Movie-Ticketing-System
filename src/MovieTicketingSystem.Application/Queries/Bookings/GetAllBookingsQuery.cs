using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Bookings
{
    public class GetAllBookingsQuery : IRequest<IEnumerable<BookingDTO>>
    {
    }
    
}
