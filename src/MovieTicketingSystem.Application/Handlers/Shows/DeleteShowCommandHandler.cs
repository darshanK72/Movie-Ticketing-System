using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Domain.Contracts.Repository;

namespace MovieTicketingSystem.Application.Commands.Shows
{
    public class DeleteShowCommandHandler : IRequestHandler<DeleteShowCommand, bool>
    {
        private readonly IShowRepository _showRepository;

        public DeleteShowCommandHandler(IShowRepository showRepository)
        {
            _showRepository = showRepository;
        }

        public async Task<bool> Handle(DeleteShowCommand request, CancellationToken cancellationToken)
        {
            return await _showRepository.DeleteShowAsync(request.Id!);
        }
    }
} 