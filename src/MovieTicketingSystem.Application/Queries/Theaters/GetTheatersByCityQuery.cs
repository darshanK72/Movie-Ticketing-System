using System.Collections.Generic;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Queries.Theaters
{
    public class GetTheatersByCityQuery : IRequest<IEnumerable<TheaterDTO>>
    {
        public string City { get; set; }

        public GetTheatersByCityQuery(string city)
        {
            City = city;
        }
    }
} 