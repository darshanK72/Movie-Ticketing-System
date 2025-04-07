using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketingSystem.Application.Queries.Search;
using MovieTicketingSystem.Domain.DTOs;

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

        [HttpGet]
        [ProducesResponseType(typeof(SearchResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery] SearchQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
