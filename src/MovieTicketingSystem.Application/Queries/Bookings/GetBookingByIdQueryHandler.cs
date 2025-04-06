using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Bookings
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDTO?>
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
            var bookingId = Guid.Parse(request.BookingId);
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
                return null;

            return _mapper.Map<BookingDTO>(booking);
        }
    }
} 