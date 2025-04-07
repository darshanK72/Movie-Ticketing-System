using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Commands.Shows
{
    public class UpdateShowCommand : IRequest<bool>
    {
        public string? Id { get; set; }
        public string? MovieId { get; set; }
        public string? CinemaHallId { get; set; }
        public bool IsActive { get; set; }
        public List<UpdateShowTimingDetails> ShowTimings { get; set; } = new();
    }

    public class UpdateShowTimingDetails
    {
        public string? Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal BasePrice { get; set; }
        public ShowStatus Status { get; set; }
        public string? ShowManagerId { get; set; }
        public bool IsActive { get; set; }
    }
} 