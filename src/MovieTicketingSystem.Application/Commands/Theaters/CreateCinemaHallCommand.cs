using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Commands.Theaters
{
    public class CreateCinemaHallCommand : IRequest<bool>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? TotalSeats { get; set; }
        public int? NumberOfRows { get; set; }
        public int? SeatsPerRow { get; set; }
        public bool? Has3D { get; set; }
        public bool? HasDolby { get; set; }
        public string? TheaterId { get; set; }
    }
} 