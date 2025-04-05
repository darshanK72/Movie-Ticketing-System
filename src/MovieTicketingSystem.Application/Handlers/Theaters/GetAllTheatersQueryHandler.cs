using System;
using System.Collections.Generic;
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
    public class GetAllTheatersQueryHandler : IRequestHandler<GetAllTheatersQuery, IEnumerable<TheaterDTO>>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMapper _mapper;

        public GetAllTheatersQueryHandler(ITheaterRepository theaterRepository,IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TheaterDTO>> Handle(GetAllTheatersQuery request, CancellationToken cancellationToken)
        {
            var theaters = await _theaterRepository.GetAllTheatersAsync();

            var theaterDtos = _mapper.Map<IEnumerable<TheaterDTO>>(theaters);

            return theaterDtos;
        }
    }
} 