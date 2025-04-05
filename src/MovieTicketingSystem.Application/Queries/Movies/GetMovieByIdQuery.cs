using System;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Movies
{
    public class GetMovieByIdQuery : IRequest<MovieDTO>
    {
        public string? Id { get; set; }

        public GetMovieByIdQuery(string Id)
        {
            this.Id = Id;
        }
    }
} 