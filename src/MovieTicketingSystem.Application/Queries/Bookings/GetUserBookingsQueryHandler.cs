using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Bookings
{
    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, List<BookingDTO>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public GetUserBookingsQueryHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<List<BookingDTO>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetUserBookingsAsync(request.UserId);
            return _mapper.Map<List<BookingDTO>>(bookings);
        }
    }
} 