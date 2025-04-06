using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetShowsByMovieQuery : IRequest<IEnumerable<ShowDTO>>
    {
        public string? MovieId { get; set; }

        public GetShowsByMovieQuery(string? movieId)
        {
            MovieId = movieId;
        }
    }
} 