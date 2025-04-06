using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Application.Commands.Shows
{
    public class UpdateShowCommandHandler : IRequestHandler<UpdateShowCommand, bool>
    {
        private readonly IShowRepository _showRepository;
         private readonly IMovieRepository _movieRepository;
        private readonly ITheaterRepository _theaterRepository;
        private readonly IValidator<UpdateShowCommand> _validator;
        private readonly IMapper _mapper;

        public UpdateShowCommandHandler(IShowRepository showRepository,
            IMovieRepository movieRepository,
            ITheaterRepository theaterRepository,
            IValidator<UpdateShowCommand> validator,
            IMapper mapper)
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateShowCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var show = await _showRepository.GetShowByIdAsync(request.Id!);
            if (show == null)
                throw new ValidationException($"Show with ID {request.Id} not found");

            var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId!);
            if (movie == null)
                throw new ValidationException($"Movie with ID {request.MovieId} not found");

            var theater = await _theaterRepository.GetTheaterByIdAsync(request.TheaterId!);
            if (theater == null)
                throw new ValidationException($"Theater with ID {request.TheaterId} not found");

            var cinemaHall = await _theaterRepository.GetCinemaHallByIdAsync(request.CinemaHallId!);
            if (cinemaHall == null)
                throw new ValidationException($"Cinema hall with ID {request.CinemaHallId} not found");

            var updatedShow = _mapper.Map<Show>(request);

            updatedShow.TotalSeats = cinemaHall.TotalSeats;
            updatedShow.AvailableSeats = cinemaHall.TotalSeats;
            
            return await _showRepository.UpdateShowAsync(updatedShow);
        }
    }
} 