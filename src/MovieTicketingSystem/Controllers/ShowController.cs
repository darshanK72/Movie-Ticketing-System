using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieTicketingSystem.Application.Commands.Shows;
using MovieTicketingSystem.Application.Queries.Shows;
using MovieTicketingSystem.Domain.Contracts.Repository;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Entities;
using AutoMapper;
using MediatR;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/shows")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShows()
        {
            var result = await _mediator.Send(new GetAllShowsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(string id)
        {
            var result = await _mediator.Send(new GetShowByIdQuery(id));
            
            if (result == null)
                return NotFound(new { Message = "Show Not Found" });

            return Ok(result);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowsByMovie(string movieId)
        {
            var result = await _mediator.Send(new GetShowsByMovieQuery(movieId));
            return Ok(result);
        }

        [HttpGet("theater/{theaterId}")]
        public async Task<IActionResult> GetShowsByTheater(string theaterId)
        {
            var result = await _mediator.Send(new GetShowsByTheaterQuery(theaterId));
            return Ok(result);
        }   

        [HttpPost]
        public async Task<IActionResult> CreateShow([FromBody] CreateShowCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest(new { Message = "Show Creation Failed" });

            return Ok(new { Message = "Show Created Successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow(string id, [FromBody] UpdateShowCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { Message = "Id Mismatch" });

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound(new { Message = "Show Not Found" });

            return Ok(new { Message = "Show Updated Successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(string id)
        {
            var result = await _mediator.Send(new DeleteShowCommand(id));
            
            if (!result)
                return NotFound(new { Message = "Show Not Found" });

            return Ok(new { Message = "Show Deleted Successfully" });
        }

        // [HttpGet("{id}/seats")]
        // public async Task<ActionResult<Show>> GetShowSeats(string id)
        // {
        //     var query = new GetShowSeatsQuery(id);
        //     var result = await _mediator.Send(query);
            
        //     if (result == null)
        //         return NotFound();

        //     return Ok(result);
        // }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingShows()
        {
            var result = await _mediator.Send(new GetUpcomingShowsQuery());
            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayShows()
        {
            var result = await _mediator.Send(new GetTodayShowsQuery());
            return Ok(result);
        }
    }
}
