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
                var booking = await _bookingRepository.GetByIdAsync(request.BookingId!);
                if (booking == null || booking.BookingStatus != BookingStatus.Pending)
                {
                    return null;
                }

                if (booking.ExpirationTime < DateTime.UtcNow)
                {
                    await _bookingRepository.CancelBookingAsync(booking.Id.ToString(), "Booking expired");
                    return null;
                }

                var paymentSuccess = await _bookingRepository.ProcessPaymentAsync(
                    booking.Id.ToString(),
                    request.PaymentMethod,
                    request.TransactionId
                );

                if (!paymentSuccess)
                {
                    await _bookingRepository.CancelBookingAsync(booking.Id.ToString(), "Payment processing failed");
                    return null;
                }

                var updatedBooking = await _bookingRepository.GetByIdAsync(booking.Id.ToString());
                
                return _mapper.Map<BookingDTO>(updatedBooking);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
} 