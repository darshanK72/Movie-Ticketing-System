using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MovieTicketingSystem.Application.Commands.Movies;
using MovieTicketingSystem.Domain.Contracts.Repository;

namespace MovieTicketingSystem.Application.Handlers.Movies
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, bool>
    {
        private readonly IMovieRepository _movieRepository;

        public DeleteMovieCommandHandler(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            return await _movieRepository.DeleteMovieAsync(request.Id!);
        }
    }
} 