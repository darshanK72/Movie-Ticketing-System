using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, string>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowSeatRepository _showSeatRepository;
        private readonly IShowTimingRepository _showTimingRepository;

        public CreateBookingCommandHandler(
            IBookingRepository bookingRepository,
            IShowSeatRepository showSeatRepository,
            IShowTimingRepository showTimingRepository)
        {
            _bookingRepository = bookingRepository;
            _showSeatRepository = showSeatRepository;
            _showTimingRepository = showTimingRepository;
        }

        public async Task<string> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ShowTimingId))
            {
                throw new ArgumentException("Show timing ID is required");
            }

            if (request.SeatIds == null || !request.SeatIds.Any())
            {
                throw new ArgumentException("At least one seat must be selected");
            }

            var showTiming = await _showTimingRepository.GetShowTimingByIdAsync(request.ShowTimingId);
            if (showTiming == null)
            {
                throw new ArgumentException("Show timing not found");
            }

            var seats = await _showSeatRepository.GetShowSeatsByShowTimingIdAsync(request.ShowTimingId);
            var selectedSeats = seats.Where(s => request.SeatIds.Contains(s.Id.ToString())).ToList();

            if (selectedSeats.Count != request.SeatIds.Count)
            {
                throw new ArgumentException("One or more selected seats are not available");
            }

            if (selectedSeats.Any(s => s.SeatBookingStatus != SeatBookingStatus.Available))
            {
                throw new ArgumentException("One or more selected seats are already booked");
            }

            var booking = await _bookingRepository.CreateInitialBookingAsync(
                request.UserId!,
                request.ShowTimingId,
                request.SeatIds);

            return booking.Id.ToString();
        }
    }
} 