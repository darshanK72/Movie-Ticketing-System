using System;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class AddressDTO
    {
        public string? Id { get; set; }
        public string? Details { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
} 