using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieTicketingSystem.Application.Commands.Shows;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Application.Validators.Shows;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Handlers.Shows
{
    public class CreateShowCommandHandler : IRequestHandler<CreateShowCommand, bool>
    {
        private readonly IShowRepository _showRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateShowCommand> _validator;

        public CreateShowCommandHandler(
            IShowRepository showRepository,
            IMapper mapper,
            IValidator<CreateShowCommand> validator)
        {
            _showRepository = showRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<bool> Handle(CreateShowCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var show = _mapper.Map<Show>(request);

            return await _showRepository.CreateShowAsync(show);
        }
    }
}