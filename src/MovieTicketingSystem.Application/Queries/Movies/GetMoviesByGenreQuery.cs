using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Movies
{
    public class GetMoviesByGenreQuery : IRequest<IEnumerable<MovieDTO>>
    {
        public string? Genre { get; set; }

        public GetMoviesByGenreQuery(string? genre)
        {
            Genre = genre;
        }
    }
} 