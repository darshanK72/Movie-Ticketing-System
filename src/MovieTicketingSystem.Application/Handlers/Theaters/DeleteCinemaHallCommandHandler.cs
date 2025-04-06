using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Shows;
using MovieTicketingSystem.Application.Commands.Theaters;
using MovieTicketingSystem.Domain.Contracts.Repository;

namespace MovieTicketingSystem.Application.Handlers.Theaters
{
    public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaHallCommand, bool>
    {
        private readonly ITheaterRepository _theaterRepository;

        public DeleteCinemaCommandHandler(ITheaterRepository theaterRepository)
        {
            _theaterRepository = theaterRepository;
        }

        public async Task<bool> Handle(DeleteCinemaHallCommand request, CancellationToken cancellationToken)
        {
            return await _theaterRepository.DeleteCinemaHallAsync(request.Id!);
        }
    }
}
