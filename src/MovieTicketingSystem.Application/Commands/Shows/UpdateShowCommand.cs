using System;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Commands.Shows
{
    public class UpdateShowCommand : IRequest<bool>
    {
        public string? Id { get; set; }
        public string? MovieId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string? TheaterId {get;set;}
        public string? CinemaHallId { get; set; }
        public decimal BasePrice { get; set; }
        public ShowStatus Status { get; set; }
        public string? ShowManagerId { get; set; }
    }
} 