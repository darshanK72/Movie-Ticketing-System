using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Handlers.Movies
{
    public class GetMoviesByRatingQueryHandler : IRequestHandler<GetMoviesByRatingQuery, IEnumerable<MovieDTO>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMoviesByRatingQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDTO>> Handle(GetMoviesByRatingQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetMoviesByRatingAsync(request.Rating);
            return _mapper.Map<IEnumerable<MovieDTO>>(movies);
        }
    }
}