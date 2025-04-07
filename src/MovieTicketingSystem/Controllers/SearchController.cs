using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketingSystem.Application.Queries.Search;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/search")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] SearchQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
