using System;
using System.Collections.Generic;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class PaymentRequestDTO
    {
        public string? BookingId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public Dictionary<string, string> PaymentDetails { get; set; } = new Dictionary<string, string>();
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        UPI,
        NetBanking,
        Wallet
    }
} 