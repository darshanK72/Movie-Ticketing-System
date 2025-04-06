using System;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Application.Queries.Shows
{
    public class GetShowByIdQuery : IRequest<ShowDTO>
    {
        public string? Id { get; set; }

        public GetShowByIdQuery(string? id)
        {
            Id = id;
        }
    }
} 