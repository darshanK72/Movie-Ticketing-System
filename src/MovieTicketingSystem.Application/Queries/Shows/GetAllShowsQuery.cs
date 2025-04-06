using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetAllShowsQuery : IRequest<IEnumerable<ShowDTO>>
    {
    }
} 