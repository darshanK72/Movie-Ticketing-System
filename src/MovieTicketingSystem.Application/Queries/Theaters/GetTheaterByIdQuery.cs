using System;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Queries.Theaters
{
    public class GetTheaterByIdQuery : IRequest<TheaterDTO>
    {
        public string? Id { get; set; }
    }
} 