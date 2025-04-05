using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieTicketingSystem.Application.Commands.Theaters;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Handlers.Theaters
{
    public class UpdateTheaterCommandHandler : IRequestHandler<UpdateTheaterCommand, bool>
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly IValidator<UpdateTheaterCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateTheaterCommandHandler(
            ITheaterRepository theaterRepository,
            IValidator<UpdateTheaterCommand> validator,
            IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateTheaterCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var address = _mapper.Map<Address>(request);
            var theater = _mapper.Map<Theater>(request);

            return await _theaterRepository.UpdateTheaterAsync(theater,address);
        }
    }
} 