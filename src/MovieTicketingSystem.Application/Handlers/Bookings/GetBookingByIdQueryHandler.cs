using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Bookings;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Handlers.Bookings
{
    internal class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDTO?>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetBookingByIdQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingDTO?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
            return booking != null ? _mapper.Map<BookingDTO>(booking) : null;
        }
    }
} 