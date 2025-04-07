using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MovieTicketingSystem.Application.Commands.Theaters;
using MovieTicketingSystem.Application.Queries.Theaters;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Authorization;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/theaters")]
    [ApiController]
    [Authorize]
    public class TheaterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TheaterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTheaters()
        {
            var query = new GetAllTheatersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTheaterById(string id)
        {
            var query = new GetTheaterByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new {Message = "Theatr Not Successfully." });
                
            return Ok(result);
        }

        [HttpPost]
        [RequireRole(UserRole.TheaterManager)]
        public async Task<IActionResult> CreateTheater([FromBody] CreateTheaterCommand command)
        {
            var result = await _mediator.Send(command);
            if (result)
                return Ok(new { Message = "Theater Created Successfully." });

            return BadRequest(new { Message = "Theater Creation Failed." });
        }

        [HttpPut("{id}")]
        [RequireRole(UserRole.TheaterManager)]
        public async Task<IActionResult> UpdateTheater(string id, [FromBody] UpdateTheaterCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Theater Updated Successfully." });

            return BadRequest(new { Message = "Theater Update Failed." });
        }

        [HttpDelete("{id}")]
        [RequireRole(UserRole.TheaterManager)]
        public async Task<IActionResult> DeleteTheater(string id)
        {
            var command = new DeleteTheaterCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Theater Not Found" });

            return Ok(new { Message = "Theater Deleted Successfully" });
        }

        [HttpGet("{id}/cinema-hall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCinemaHalls(string id)
        {
            var query = new GetCinemaHallsByTheaterIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpGet("{id}/cinema-hall/{cinemaHallId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCinemaHallById(string id,string cinemaHallId)
        {
            var query = new GetCinemaHallByIdQuery(cinemaHallId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("{id}/cinema-hall")]
        [RequireRole(UserRole.TheaterManager)]
        public async Task<IActionResult> CreateCinemaHall(string id, [FromBody] CreateCinemaHallCommand command)
        {
            command.TheaterId = id;
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Cinema Hall Created Successfully." });

            return BadRequest(new { Message = "Cinema Hall Creation Failed." });
        }

        [HttpPut("{id}/cinema-hall/{cinemaHallId}")]
        [RequireRole(UserRole.TheaterManager)]
        public async Task<IActionResult> UpdateCinemaHall(string id, string cinemaHallId, [FromBody] UpdateCinemaHallCommand command)
        {
            command.Id = cinemaHallId;
            var result = await _mediator.Send(command);

            if (result)
                return Ok(new { Message = "Cinema Hall Updated Successfully." });

            return BadRequest(new { Message = "Cinema Hall Update Failed." });
        }

        [HttpDelete("{id}/cinema-hall/{cinemaHallId}")]
        [RequireRole(UserRole.TheaterManager)]
        public async Task<IActionResult> DeleteCinemaHall(string id,string cinemaHallId)
        {
            var command = new DeleteCinemaHallCommand(cinemaHallId);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound(new { Message = "Theater Not Found" });

            return Ok(new { Message = "Theater Deleted Successfully" });
        }

    }
}
