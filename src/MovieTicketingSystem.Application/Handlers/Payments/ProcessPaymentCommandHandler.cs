using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Payments;
using MovieTicketingSystem.Application.Repositories;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Commands.Payments
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentDTO>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentRepository _paymentRepository;

        public ProcessPaymentCommandHandler(
            IBookingRepository bookingRepository,
            IPaymentRepository paymentRepository)
        {
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDTO> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId!);
            if (booking == null)
                throw new Exception("Booking not found");

            if (booking.TotalAmount != request.Amount)
                throw new Exception("Payment amount does not match booking amount");

            if (booking.PaymentStatus == PaymentStatus.Completed)
                throw new Exception("Payment already completed for this booking");

            if (booking.ExpirationTime < DateTime.UtcNow)
                throw new Exception("Booking has expired");

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod.ToString(),
                TransactionId = GenerateTransactionId(),
                PaymentStatus = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await SimulatePaymentProcessing(payment, request.PaymentDetails);
                
                payment.PaymentStatus = PaymentStatus.Completed;
                payment.UpdatedAt = DateTime.UtcNow;
                
                await _paymentRepository.CreatePaymentAsync(payment);
                booking.BookingStatus = BookingStatus.Completed;
                booking.PaymentStatus = PaymentStatus.Completed;
                booking.UpdatedAt = DateTime.UtcNow;

                foreach(var seat in booking.ShowSeats!)
                {
                    seat.SeatBookingStatus = SeatBookingStatus.Booked;
                }

                await _bookingRepository.UpdateBookingAsync(booking);
                
                return new PaymentDTO
                {
                    Id = payment.Id.ToString(),
                    BookingId = payment.BookingId.ToString(),
                    Amount = payment.Amount,
                    PaymentMethod = payment.PaymentMethod,
                    TransactionId = payment.TransactionId,
                    PaymentStatus = payment.PaymentStatus.ToString(),
                    PaymentDate = payment.PaymentDate,
                    RefundDate = payment.RefundDate,
                    RefundReason = payment.RefundReason,
                    FailureReason = payment.FailureReason
                };
            }
            catch (Exception ex)
            {
                payment.PaymentStatus = PaymentStatus.Failed;
                payment.FailureReason = ex.Message;
                payment.UpdatedAt = DateTime.UtcNow;
                await _paymentRepository.CreatePaymentAsync(payment);
                
                throw new Exception($"Payment failed: {ex.Message}");
            }
        }

        private string GenerateTransactionId()
        {
            return $"TXN{DateTime.UtcNow.Ticks}";
        }

        private async Task SimulatePaymentProcessing(Payment payment, Dictionary<string, string> paymentDetails)
        {
            await Task.Delay(1000);
            
            switch (payment.PaymentMethod)
            {
                case "CreditCard":
                case "DebitCard":
                    if (!paymentDetails.ContainsKey("CardNumber") || 
                        !paymentDetails.ContainsKey("CardHolderName") || 
                        !paymentDetails.ContainsKey("ExpiryDate") || 
                        !paymentDetails.ContainsKey("CVV"))
                        throw new Exception("Missing required card details");
                    break;
                    
                case "UPI":
                    if (!paymentDetails.ContainsKey("UPIId"))
                        throw new Exception("Missing UPI ID");
                    break;
                    
                case "NetBanking":
                    if (!paymentDetails.ContainsKey("BankName") || 
                        !paymentDetails.ContainsKey("AccountNumber"))
                        throw new Exception("Missing bank details");
                    break;
                    
                case "Wallet":
                    if (!paymentDetails.ContainsKey("WalletType") || 
                        !paymentDetails.ContainsKey("WalletId"))
                        throw new Exception("Missing wallet details");
                    break;
                    
                default:
                    throw new Exception("Invalid payment method");
            }
        }
    }
} 