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

namespace MovieTicketingSystem.Application.Handlers.Shows
{
    public class UpdateShowCommandHandler : IRequestHandler<UpdateShowCommand, bool>
    {
        private readonly IShowRepository _showRepository;
        private readonly IShowTimingRepository _showTimingRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateShowCommand> _validator;

        public UpdateShowCommandHandler(
            IShowRepository showRepository,
            IShowTimingRepository showTimingRepository,
            IMapper mapper,
            IValidator<UpdateShowCommand> validator)
        {
            _showRepository = showRepository;
            _showTimingRepository = showTimingRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<bool> Handle(UpdateShowCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Update the show
            var show = _mapper.Map<Show>(request);
            var showUpdated = await _showRepository.UpdateShowAsync(show);
            if (!showUpdated)
            {
                return false;
            }

            foreach (var timingDetails in request.ShowTimings)
            {
                var showTiming = new ShowTiming
                {
                    Id = Guid.Parse(timingDetails.Id!),
                    ShowId = show.Id,
                    Date = timingDetails.Date,
                    StartTime = timingDetails.StartTime,
                    EndTime = timingDetails.EndTime,
                    BasePrice = timingDetails.BasePrice,
                    ShowStatus = timingDetails.Status,
                    ShowManagerId = timingDetails.ShowManagerId,
                    IsActive = timingDetails.IsActive,
                    UpdatedAt = DateTime.UtcNow
                };

                await _showTimingRepository.UpdateShowTimingAsync(showTiming);
            }

            return true;
        }
    }
} 