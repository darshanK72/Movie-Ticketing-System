using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Shows;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Contracts.Repository;

namespace MovieTicketingSystem.Application.Handlers.Shows
{
    public class GetShowSeatsByShowTimingIdQueryHandler : IRequestHandler<GetShowSeatsByShowTimingIdQuery, IEnumerable<ShowSeatDTO>>
    {
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;

        public GetShowSeatsByShowTimingIdQueryHandler(IShowRepository showRepository, IMapper mapper)
        {
            _showRepository = showRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShowSeatDTO>> Handle(GetShowSeatsByShowTimingIdQuery request, CancellationToken cancellationToken)
        {
            var showSeats = await _showRepository.GetShowSeatsByShowTimingIdAsync(request.ShowTimingId);
            return _mapper.Map<IEnumerable<ShowSeatDTO>>(showSeats);
        }
    }
} 