using System;
using MediatR;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Commands.Shows
{
    public class CreateShowTimingCommand : IRequest<bool>
    {
        public Guid ShowId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal BasePrice { get; set; }
        public ShowStatus Status { get; set; }
        public string? ShowManagerId { get; set; }
    }
} 