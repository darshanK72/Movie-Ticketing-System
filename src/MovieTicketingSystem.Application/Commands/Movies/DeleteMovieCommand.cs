using System;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Movies
{
    public class DeleteMovieCommand : IRequest<bool>
    {
        public string? Id { get; set; }

        public DeleteMovieCommand(string? id)
        {
            Id = id;
        }
    }
} 