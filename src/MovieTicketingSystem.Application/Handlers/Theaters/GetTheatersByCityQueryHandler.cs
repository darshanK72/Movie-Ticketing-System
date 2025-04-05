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
    public class GetTheatersByCityQueryHandler : IRequestHandler<GetTheatersByCityQuery, IEnumerable<TheaterDTO>>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMapper _mapper;

        public GetTheatersByCityQueryHandler(ITheaterRepository theaterRepository, IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TheaterDTO>> Handle(GetTheatersByCityQuery request, CancellationToken cancellationToken)
        {
            var theaters =  await _theaterRepository.GetTheatersByCityAsync(request.City);
            var theaterDtos = _mapper.Map<IEnumerable<TheaterDTO>>(theaters);
            return theaterDtos;
        }
    }
} 