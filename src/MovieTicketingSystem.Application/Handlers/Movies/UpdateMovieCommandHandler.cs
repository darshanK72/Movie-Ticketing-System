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
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, bool>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<UpdateMovieCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateMovieCommandHandler(
            IMovieRepository movieRepository,
            IValidator<UpdateMovieCommand> validator,
            IMapper mapper)
        {
            _movieRepository = movieRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingMovie = await _movieRepository.GetMovieByIdAsync(request.Id!);
            if (existingMovie == null)
            {
                return false;
            }

            _mapper.Map(request, existingMovie);
            existingMovie.UpdatedAt = DateTime.UtcNow;

            return await _movieRepository.UpdateMovieAsync(existingMovie);
        }
    }
} 