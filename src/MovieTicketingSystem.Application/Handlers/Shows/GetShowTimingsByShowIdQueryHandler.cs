using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Shows;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Contracts.Repository;

namespace MovieTicketingSystem.Application.Handlers.Shows
{
    public class GetShowTimingsByShowIdQueryHandler : IRequestHandler<GetShowTimingsByShowIdQuery, IEnumerable<ShowTimingDTO>>
    {
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;

        public GetShowTimingsByShowIdQueryHandler(IShowRepository showRepository, IMapper mapper)
        {
            _showRepository = showRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShowTimingDTO>> Handle(GetShowTimingsByShowIdQuery request, CancellationToken cancellationToken)
        {
            var showTimings = await _showRepository.GetShowTimingsByShowIdAsync(request.ShowId);
            return _mapper.Map<IEnumerable<ShowTimingDTO>>(showTimings);
        }
    }
} 