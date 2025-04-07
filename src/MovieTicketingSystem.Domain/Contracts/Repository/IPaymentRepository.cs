using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Domain.Contracts.Repository
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetPaymentByIdAsync(string id);
        Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(string bookingId);
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<bool> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(string id);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status);
    }
} 