using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Handlers.Movies
{
    public class GetActiveMoviesQueryHandler : IRequestHandler<GetActiveMoviesQuery, IEnumerable<MovieDTO>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetActiveMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDTO>> Handle(GetActiveMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetActiveMoviesAsync();
            return _mapper.Map<IEnumerable<MovieDTO>>(movies);
        }
    }
}