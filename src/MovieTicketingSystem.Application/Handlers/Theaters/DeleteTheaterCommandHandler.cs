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
    public class DeleteTheaterCommandHandler : IRequestHandler<DeleteTheaterCommand, bool>
    {
        private readonly ITheaterRepository _theaterRepository;

        public DeleteTheaterCommandHandler(ITheaterRepository theaterRepository)
        {
            _theaterRepository = theaterRepository;
        }

        public async Task<bool> Handle(DeleteTheaterCommand request, CancellationToken cancellationToken)
        {
            return await _theaterRepository.DeleteTheaterAsync(request.Id!);
        }
    }
}
