using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Persistence;

namespace MovieTicketingSystem.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly TicketingDbContext _context;

        public PaymentRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetPaymentByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid paymentId))
                return null;

            return await _context.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBookingIdAsync(string bookingId)
        {
            if (!Guid.TryParse(bookingId, out Guid bookingGuid))
                return Enumerable.Empty<Payment>();

            return await _context.Payments
                .Where(p => p.BookingId == bookingGuid)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            try
            {
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeletePaymentAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid paymentId))
                return false;

            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status)
        {
            if (!Enum.TryParse<PaymentStatus>(status, out var paymentStatus))
                return Enumerable.Empty<Payment>();

            return await _context.Payments
                .Where(p => p.PaymentStatus == paymentStatus)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
} 