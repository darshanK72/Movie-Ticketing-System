using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieTicketingSystem.Application.Commands.Theaters;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Handlers.Theaters
{
    public class CreateCinemaHallCommandHandler : IRequestHandler<CreateCinemaHallCommand, bool>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IValidator<CreateCinemaHallCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ISeatGenerationService _seatGenerationService;

        public CreateCinemaHallCommandHandler(
            ITheaterRepository theaterRepository,
            IValidator<CreateCinemaHallCommand> validator,
            IMapper mapper,
            ISeatGenerationService seatGenerationService)
        {
            _theaterRepository = theaterRepository;
            _validator = validator;
            _mapper = mapper;
            _seatGenerationService = seatGenerationService;
        }

        public async Task<bool> Handle(CreateCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var cinemaHall = _mapper.Map<CinemaHall>(request);
            cinemaHall.CreatedAt = DateTime.UtcNow;
            cinemaHall.IsActive = true;

            var seats = _seatGenerationService.GenerateSeatsForCinemaHall(cinemaHall);
            return await _theaterRepository.CreateCinemaHallWithSeatsAsync(cinemaHall, seats);
        }
    }
} 