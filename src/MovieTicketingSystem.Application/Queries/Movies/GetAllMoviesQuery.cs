using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Movies
{
    public class GetAllMoviesQuery : IRequest<IEnumerable<MovieDTO>>
    {
    }
} 