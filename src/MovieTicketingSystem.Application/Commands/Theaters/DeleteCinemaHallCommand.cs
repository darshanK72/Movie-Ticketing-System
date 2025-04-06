using MediatR;

namespace MovieTicketingSystem.Application.Commands.Theaters
{
    public class DeleteCinemaHallCommand : IRequest<bool>
    {
        public string? Id { get; set; }

        public DeleteCinemaHallCommand(string id){
            Id = id;
        }
    }
}