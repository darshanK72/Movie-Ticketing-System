using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
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
        private readonly IShowTimingRepository _showTimingRepository;
        private readonly IShowSeatRepository _showSeatRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateShowCommand> _validator;

        public CreateShowCommandHandler(
            IShowRepository showRepository,
            IShowTimingRepository showTimingRepository,
            IShowSeatRepository showSeatRepository,
            IMapper mapper,
            IValidator<CreateShowCommand> validator)
        {
            _showRepository = showRepository;
            _showTimingRepository = showTimingRepository;
            _showSeatRepository = showSeatRepository;
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
            var showCreated = await _showRepository.CreateShowAsync(show);
            if (!showCreated)
            {
                return false;
            }

            foreach (var timingDetails in request.ShowTimings)
            {
                var showTiming = new ShowTiming
                {
                    ShowId = show.Id,
                    Date = timingDetails.Date,
                    StartTime = timingDetails.StartTime,
                    EndTime = timingDetails.EndTime,
                    BasePrice = timingDetails.BasePrice,
                    ShowStatus = timingDetails.Status,
                    ShowManagerId = timingDetails.ShowManagerId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var timingCreated = await _showTimingRepository.CreateShowTimingAsync(showTiming);
                if (!timingCreated)
                {
                    continue;
                }

                var seats = await _showSeatRepository.GetSeatsByCinemaHallAsync(show.CinemaHallId);
                foreach (var seat in seats)
                {
                    var showSeat = new ShowSeat
                    {
                        ShowTimingId = showTiming.Id,
                        SeatId = seat.Id,
                        IsBooked = false,
                        SeatBookingStatus = SeatBookingStatus.Available,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _showSeatRepository.CreateShowSeatAsync(showSeat);
                }

                showTiming.TotalSeats = seats.Count();
                showTiming.AvailableSeats = seats.Count();
                await _showTimingRepository.UpdateShowTimingAsync(showTiming);
            }

            return true;
        }
    }
}