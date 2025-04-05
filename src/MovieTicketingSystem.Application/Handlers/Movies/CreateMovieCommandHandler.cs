using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieTicketingSystem.Application.Commands.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Handlers.Movies
{
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, bool>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<CreateMovieCommand> _validator;
        private readonly IMapper _mapper;

        public CreateMovieCommandHandler(
            IMovieRepository movieRepository,
            IValidator<CreateMovieCommand> validator,
            IMapper mapper)
        {
            _movieRepository = movieRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var movie = _mapper.Map<Movie>(request);
            movie.CreatedAt = DateTime.UtcNow;
            movie.IsActive = true;

            return await _movieRepository.CreateMovieAsync(movie);
        }
    }
} 