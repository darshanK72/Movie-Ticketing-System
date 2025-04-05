using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Application.Queries.Movies
{
    public class GetMoviesByRatingQuery : IRequest<IEnumerable<MovieDTO>>
    {
        public MovieRating Rating { get; set; }

        public GetMoviesByRatingQuery(MovieRating rating)
        {
            Rating = rating;
        }
    }
} 