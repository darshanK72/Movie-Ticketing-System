 using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MovieTicketingSystem.Application.Queries.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Handlers.Movies
{
    public class GetMoviesByLanguageQuery : IRequest<IEnumerable<MovieDTO>>
    {
        public string? Language { get; set; }

        public GetMoviesByLanguageQuery(string? language)
        {
            Language = language;
        }
    }
}