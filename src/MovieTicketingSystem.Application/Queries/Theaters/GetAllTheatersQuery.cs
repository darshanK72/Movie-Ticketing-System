using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Queries.Theaters
{
    public class GetAllTheatersQuery : IRequest<IEnumerable<TheaterDTO>>
    {
    }
} 