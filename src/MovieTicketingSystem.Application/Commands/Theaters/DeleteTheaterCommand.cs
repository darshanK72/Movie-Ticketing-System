using System;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Theaters
{
    public class DeleteTheaterCommand : IRequest<bool>
    {
        public string? Id { get; set; }

        public DeleteTheaterCommand(string id){
            Id = id;
        }
    }
} 