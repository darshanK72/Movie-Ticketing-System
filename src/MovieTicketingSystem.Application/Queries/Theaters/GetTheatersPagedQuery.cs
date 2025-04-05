using System;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Common;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Queries.Theaters
{
    public class GetTheatersPagedQuery : IRequest<PagedResult<TheaterDTO>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }
} 