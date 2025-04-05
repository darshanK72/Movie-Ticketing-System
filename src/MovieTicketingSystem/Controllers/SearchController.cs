using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketingSystem.Application.Queries.Theaters;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetTheatersPaged([FromQuery] GetTheatersPagedQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetTheatersByCity(string city)
        {
            var query = new GetTheatersByCityQuery(city);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
