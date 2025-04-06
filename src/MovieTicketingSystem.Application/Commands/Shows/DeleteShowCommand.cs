using System;
using System.Threading.Tasks;
using MediatR;

namespace MovieTicketingSystem.Application.Commands.Shows
{
    public class DeleteShowCommand : IRequest<bool>
    {
        public string? Id { get; set; }

        public DeleteShowCommand(string id){
            Id = id;
        }
    }
} 