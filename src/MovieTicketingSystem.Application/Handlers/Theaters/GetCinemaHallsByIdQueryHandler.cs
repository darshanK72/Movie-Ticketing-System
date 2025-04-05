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
    public class GetCinemaHallByIdQueryHandler : IRequestHandler<GetCinemaHallByIdQuery, CinemaHallDTO>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMapper _mapper;

        public GetCinemaHallByIdQueryHandler(ITheaterRepository theaterRepository, IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _mapper = mapper;
        }

        public async Task<CinemaHallDTO> Handle(GetCinemaHallByIdQuery request, CancellationToken cancellationToken)
        {
            var cinemaHall = await _theaterRepository.GetCinemaHallByIdAsync(request.Id!);
            return _mapper.Map<CinemaHallDTO>(cinemaHall);
        }
    }
} 