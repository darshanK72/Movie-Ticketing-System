using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Theaters;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Handlers.Theaters
{
    public class GetTheaterByIdQueryHandler : IRequestHandler<GetTheaterByIdQuery, TheaterDTO>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMapper _mapper;

        public GetTheaterByIdQueryHandler(ITheaterRepository theaterRepository,IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _mapper = mapper;
        }

        public async Task<TheaterDTO> Handle(GetTheaterByIdQuery request, CancellationToken cancellationToken)
        {
            var theater = await _theaterRepository.GetTheaterByIdAsync(request.Id!);
            var theaterDto = _mapper.Map<TheaterDTO>(theater);
            return theaterDto;
        }
    }
} 