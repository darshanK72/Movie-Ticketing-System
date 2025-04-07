using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Bookings;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.Contracts.Services;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Repository;

namespace MovieTicketingSystem.Application.Handlers.Bookings
{
    public class DownloadBookingTicketCommandHandler : IRequestHandler<DownloadBookingTicketCommand, byte[]>
    {
        private readonly ITicketPdfService _ticketPdfService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowTimingRepository _showTimingRepository;
 

        public DownloadBookingTicketCommandHandler(
            ITicketPdfService ticketPdfService, 
            IBookingRepository bookingRepository,
            IShowTimingRepository showTimingRepository)
        {
            _ticketPdfService = ticketPdfService;
            _bookingRepository = bookingRepository;
            _showTimingRepository = showTimingRepository;
        }

        public async Task<byte[]> Handle(DownloadBookingTicketCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.Id);
            var showTiming = await _showTimingRepository.GetShowTimingByIdAsync(booking!.ShowTimingId.ToString()!);
            if (booking == null)
                return Array.Empty<byte>();

            var ticketDto = new TicketDTO
            {
                BookingId = booking.Id.ToString(),
                MovieTitle = showTiming?.Show?.Movie?.Title ?? "Unknown Movie",
                TheaterName = showTiming?.Show?.CinemaHall?.Theater?.Name ?? "Unknown Theater",
                CinemaHallName = showTiming?.Show?.CinemaHall?.Name ?? "Unknown Hall",
                ShowDateTime = showTiming?.Date.Add(showTiming.StartTime) ?? DateTime.MinValue,
                SeatNumbers = booking.ShowSeats!.Select(s => s.Seat?.SeatNumber!).ToList() ?? new List<string>(),
                NumberOfTickets = booking.NumberOfTickets,
                TotalAmount = booking.TotalAmount,
                UserName = $"{booking.User?.FirstName} {booking.User?.LastName}".Trim(),
                UserEmail = booking.User?.Email ?? "Unknown Email",
                BookingStatus = booking.BookingStatus.ToString(),
                PaymentStatus = booking.PaymentStatus.ToString(),
                BookingDate = booking.BookingDate
            };

            return _ticketPdfService.GenerateTicketPdf(ticketDto);
        }
    }
}
