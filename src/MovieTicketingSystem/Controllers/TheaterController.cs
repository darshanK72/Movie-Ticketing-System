using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MovieTicketingSystem.Application.Commands.Theaters;
using MovieTicketingSystem.Application.Queries.Theaters;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/theaters")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TheaterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTheaters()
        {
            var query = new GetAllTheatersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheaterById(string id)
        {
            var query = new GetTheaterByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTheater([FromBody] CreateTheaterCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok(new { Message = "Theater Created Successfully." });

            return BadRequest(new { Message = "Theater Creation Failed." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTheater(string id, [FromBody] UpdateTheaterCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Theater Updated Successfully." });

            return BadRequest(new { Message = "Theater Update Failed." });
        }

        [HttpGet("{id}/cinema-hall")]
        public async Task<IActionResult> GetAllCinemaHalls(string id)
        {
            var query = new GetCinemaHallsByTheaterIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheater(string id)
        {
            var command = new DeleteTheaterCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Theater Not Found" });

            return Ok(new { Message = "Theater Deleted Successfully" });
        }

        [HttpGet("{id}/cinema-hall/{cinemaHallId}")]
        public async Task<IActionResult> GetCinemaHallById(string id,string cinemaHallId)
        {
            var query = new GetCinemaHallByIdQuery(cinemaHallId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/cinema-hall")]
        public async Task<IActionResult> CreateCinemaHall(string id, [FromBody] CreateCinemaHallCommand command)
        {
            command.TheaterId = id;
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Cinema Hall Created Successfully." });

            return BadRequest(new { Message = "Cinema Hall Creation Failed." });
        }

        [HttpPut("{id}/cinema-hall/{cinemaHallId}")]
        public async Task<IActionResult> UpdateCinemaHall(string id, string cinemaHallId, [FromBody] UpdateCinemaHallCommand command)
        {
            command.Id = cinemaHallId;
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Cinema Hall Updated Successfully." });

            return BadRequest(new { Message = "Cinema Hall Update Failed." });
        }

        [HttpDelete("{id}/cinema-hall/{cinemaHallId}")]
        public async Task<IActionResult> DeleteCinemaHall(string cinemaHallId)
        {
            var command = new DeleteTheaterCommand { Id = cinemaHallId };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Theater Not Found" });

            return Ok(new { Message = "Theater Deleted Successfully" });
        }

    }
}
