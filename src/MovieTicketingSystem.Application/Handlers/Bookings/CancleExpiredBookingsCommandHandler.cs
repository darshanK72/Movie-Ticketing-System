using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Bookings;
using MovieTicketingSystem.Application.Repositories;

namespace MovieTicketingSystem.Application.Handlers.Bookings
{
    public class CancleExpiredBookingsCommandHandler : IRequestHandler<CancleExpiredBookingsCommand, bool>
    {
        private readonly IBookingRepository _bookingRepository;

        public CancleExpiredBookingsCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> Handle(CancleExpiredBookingsCommand request, CancellationToken cancellationToken)
        {
            return await _bookingRepository.CancelExpiredBookingsAsync();

        }
    }
}
