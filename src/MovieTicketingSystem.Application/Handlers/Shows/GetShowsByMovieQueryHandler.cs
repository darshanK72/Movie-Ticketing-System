using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetShowsByMovieQueryHandler : IRequestHandler<GetShowsByMovieQuery, IEnumerable<ShowDTO>>
    {
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;

        public GetShowsByMovieQueryHandler(IShowRepository showRepository, IMapper mapper)
        {
            _showRepository = showRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShowDTO>> Handle(GetShowsByMovieQuery request, CancellationToken cancellationToken)
        {
            var shows = await _showRepository.GetShowsByMovieAsync(request.MovieId);
            return _mapper.Map<IEnumerable<ShowDTO>>(shows);
        }
    }
} 