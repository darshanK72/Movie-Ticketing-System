using System;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Commands.Theaters
{
    public class CreateTheaterCommand : IRequest<bool>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? AddressDetails { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
    }
} 