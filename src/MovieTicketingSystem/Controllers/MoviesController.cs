using Microsoft.AspNetCore.Mvc;
using MediatR;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Application.Queries.Movies;
using MovieTicketingSystem.Application.Commands.Movies;
using MovieTicketingSystem.Application.Handlers.Movies;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var query = new GetAllMoviesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(string id)
        {
            var query = new GetMovieByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new { Message = "Movie Not Found" });
                
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok(new { Message = "Movie Created Successfully." });

            return BadRequest(new { Message = "Movie Creation Failed." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(string id, [FromBody] UpdateMovieCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Movie Updated Successfully." });

            return BadRequest(new { Message = "Movie Update Failed." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            var command = new DeleteMovieCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Movie Not Found" });

            return Ok(new { Message = "Movie Deleted Successfully" });
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveMovies()
        {
            var query = new GetActiveMoviesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("genre/{genre}")]
        public async Task<IActionResult> GetMoviesByGenre(string genre)
        {
            var query = new GetMoviesByGenreQuery(genre);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("language/{language}")]
        public async Task<IActionResult> GetMoviesByLanguage(string language)
        {
            var query = new GetMoviesByLanguageQuery(language);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("rating/{rating}")]
        public async Task<IActionResult> GetMoviesByRating(MovieRating rating)
        {
            var query = new GetMoviesByRatingQuery(rating);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
} 