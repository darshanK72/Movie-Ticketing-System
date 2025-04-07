using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Commands.Shows
{
    public class CreateShowCommand : IRequest<bool>
    {
        public Guid MovieId { get; set; }
        public Guid CinemaHallId { get; set; }
        public decimal BasePrice { get; set; }
        public List<ShowTimingDetails> ShowTimings { get; set; } = new();
    }

    public class ShowTimingDetails
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal BasePrice { get; set; }
        public ShowStatus Status { get; set; }
        public string? ShowManagerId { get; set; }
    }
} 