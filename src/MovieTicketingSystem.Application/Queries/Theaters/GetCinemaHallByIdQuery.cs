using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Theaters
{
    public class GetCinemaHallByIdQuery : IRequest<CinemaHallDTO>
    {
        public string? Id { get; set; }

        public GetCinemaHallByIdQuery(string? id)
        {
            Id = id;
        }
    }
}
