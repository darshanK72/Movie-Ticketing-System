using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieTicketingSystem.Application.Commands.Theaters;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Handlers.Theaters
{
    public class UpdateCinemaHallCommandHandler : IRequestHandler<UpdateCinemaHallCommand, bool>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IValidator<UpdateCinemaHallCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateCinemaHallCommandHandler(
            ITheaterRepository theaterRepository,
            IValidator<UpdateCinemaHallCommand> validator,
            IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingCinemaHall = await _theaterRepository.GetCinemaHallByIdAsync(request.Id!);
            if (existingCinemaHall == null)
            {
                return false;
            }

            var cinemaHall = _mapper.Map<CinemaHall>(request);
            return await _theaterRepository.UpdateCinemaHallAsync(cinemaHall);
        }
    }
}
