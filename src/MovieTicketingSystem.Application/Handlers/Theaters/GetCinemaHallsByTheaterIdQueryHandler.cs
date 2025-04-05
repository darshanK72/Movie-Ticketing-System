using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Theaters;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Handlers.Theaters
{
    public class GetCinemaHallsByTheaterIdQueryHandler : IRequestHandler<GetCinemaHallsByTheaterIdQuery, IEnumerable<CinemaHallDTO>>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMapper _mapper;

        public GetCinemaHallsByTheaterIdQueryHandler(ITheaterRepository theaterRepository, IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CinemaHallDTO>> Handle(GetCinemaHallsByTheaterIdQuery request, CancellationToken cancellationToken)
        {
            var cinemaHalls = await _theaterRepository.GetCinemaHallsByTheaterIdAsync(request.Id!);
            return _mapper.Map<List<CinemaHallDTO>>(cinemaHalls);
        }
    }
} 