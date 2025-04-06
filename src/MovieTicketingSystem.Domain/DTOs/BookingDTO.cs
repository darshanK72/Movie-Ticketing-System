using System;
using System.Collections.Generic;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class BookingDTO
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? ShowId { get; set; }
        public List<ShowSeatDTO>? ShowSeats { get; set; }
        public int NumberOfTickets { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? CancellationDate { get; set; }
        public string? CancellationReason { get; set; }
        public List<PaymentDTO>? Payments { get; set; }
    }
} 