using System;
using System.Collections.Generic;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class TheaterDTO
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public AddressDTO? Address { get; set; }
        public ICollection<CinemaHallDTO>? CinemaHalls { get; set; }
    }
} 