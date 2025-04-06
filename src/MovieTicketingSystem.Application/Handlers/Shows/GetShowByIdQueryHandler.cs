using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetShowByIdQueryHandler : IRequestHandler<GetShowByIdQuery, ShowDTO>
    {
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;

        public GetShowByIdQueryHandler(IShowRepository showRepository, IMapper mapper)
        {
            _showRepository = showRepository;
            _mapper = mapper;
        }

        public async Task<ShowDTO> Handle(GetShowByIdQuery request, CancellationToken cancellationToken)
        {
            var show = await _showRepository.GetShowByIdAsync(request.Id);
            return _mapper.Map<ShowDTO>(show);
        }
    }
} 