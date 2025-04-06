using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class CancleExpiredBookingsCommand : IRequest<bool>
    {
    }
}
