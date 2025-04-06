using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetShowsByTheaterQuery : IRequest<IEnumerable<ShowDTO>>
    {
        public string? TheaterId { get; set; }

        public GetShowsByTheaterQuery(string? theaterId)
        {
            TheaterId = theaterId;
        }
    }
} 