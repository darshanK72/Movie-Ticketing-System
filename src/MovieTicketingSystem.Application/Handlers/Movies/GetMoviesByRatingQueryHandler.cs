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
    public class GetMoviesByLanguageQueryHandler : IRequestHandler<GetMoviesByLanguageQuery, IEnumerable<MovieDTO>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMoviesByLanguageQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDTO>> Handle(GetMoviesByLanguageQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetMoviesByLanguageAsync(request.Language!);
            return _mapper.Map<IEnumerable<MovieDTO>>(movies);
        }
    }
}