using System;
using System.Text.Json.Serialization;

namespace MovieTicketingSystem.Domain.DTOs
{
    public class SeatDTO
    {
        public string? Id { get; set; }
        public string? CinemaHallId { get; set; }
        public string? SeatNumber { get; set; }
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public string? Type { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
} 