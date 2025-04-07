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
using Microsoft.AspNetCore.Authorization;
using MovieTicketingSystem.Infrastructure.Authorization;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/shows")]
    [ApiController]
    [Authorize]
    public class ShowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllShows()
        {
            var result = await _mediator.Send(new GetAllShowsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowById(string id)
        {
            var result = await _mediator.Send(new GetShowByIdQuery(id));
            
            if (result == null)
                return NotFound(new { Message = "Show Not Found" });

            return Ok(result);
        }

        [HttpGet("movie/{movieId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowsByMovie(string movieId)
        {
            var result = await _mediator.Send(new GetShowsByMovieQuery(movieId));
            return Ok(result);
        }

        [HttpGet("theater/{theaterId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowsByTheater(string theaterId)
        {
            var result = await _mediator.Send(new GetShowsByTheaterQuery(theaterId));
            return Ok(result);
        }   

        [HttpPost]
        [RequireRole(UserRole.ShowManager)]
        public async Task<IActionResult> CreateShow([FromBody] CreateShowCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest(new { Message = "Show Creation Failed" });

            return Ok(new { Message = "Show Created Successfully" });
        }

        [HttpPut("{id}")]
        [RequireRole(UserRole.ShowManager)]
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
        [RequireRole(UserRole.ShowManager)]
        public async Task<IActionResult> DeleteShow(string id)
        {
            var result = await _mediator.Send(new DeleteShowCommand(id));
            
            if (!result)
                return NotFound(new { Message = "Show Not Found" });

            return Ok(new { Message = "Show Deleted Successfully" });
        }

        [HttpGet("upcoming")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUpcomingShows()
        {
            var result = await _mediator.Send(new GetUpcomingShowsQuery());
            return Ok(result);
        }

        [HttpGet("today")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTodayShows()
        {
            var result = await _mediator.Send(new GetTodayShowsQuery());
            return Ok(result);
        }

        [HttpGet("{showId}/timings")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowTimings(string showId)
        {
            if (!Guid.TryParse(showId, out Guid guid))
                return BadRequest(new { Message = "Invalid Show ID format" });

            var result = await _mediator.Send(new GetShowTimingsByShowIdQuery(guid));
            return Ok(result);
        }

        [HttpGet("timings/{showTimingId}/seats")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowSeats(string showTimingId)
        {
            if (!Guid.TryParse(showTimingId, out Guid guid))
                return BadRequest(new { Message = "Invalid Show Timing ID format" });

            var result = await _mediator.Send(new GetShowSeatsByShowTimingIdQuery(guid));
            return Ok(result);
        }
    }
}
