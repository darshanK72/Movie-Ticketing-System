using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Commands.Bookings
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, BookingDTO>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public ProcessPaymentCommandHandler(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<BookingDTO> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the booking
                var booking = await _bookingRepository.GetByIdAsync(request.BookingId);
                if (booking == null || booking.Status != BookingStatus.Pending)
                {
                    return null;
                }

                // Check if booking has expired (5 minutes)
                if (booking.ExpirationTime < DateTime.UtcNow)
                {
                    await _bookingRepository.CancelBookingAsync(booking.Id, "Booking expired");
                    return null;
                }

                // Process the payment
                var paymentSuccess = await _bookingRepository.ProcessPaymentAsync(
                    booking.Id,
                    request.PaymentMethod,
                    request.TransactionId
                );

                if (!paymentSuccess)
                {
                    await _bookingRepository.CancelBookingAsync(booking.Id, "Payment processing failed");
                    return null;
                }

                // Get the updated booking with all related data
                var updatedBooking = await _bookingRepository.GetByIdAsync(booking.Id);
                
                // Map to DTO and return
                return _mapper.Map<BookingDTO>(updatedBooking);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
} 