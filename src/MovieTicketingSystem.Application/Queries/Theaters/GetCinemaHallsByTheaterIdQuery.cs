using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Theaters
{
    public class GetCinemaHallsByTheaterIdQuery : IRequest<IEnumerable<CinemaHallDTO>>
    {
        public string? Id { get; set; }

        public GetCinemaHallsByTheaterIdQuery(string? id)
        {
            Id = id;
        }
    }
}
