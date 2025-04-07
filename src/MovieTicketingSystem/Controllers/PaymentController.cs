using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketingSystem.Application.Commands.Payments;
using MovieTicketingSystem.Application.Queries.Payments;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/payments")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("methods")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var query = new GetPaymentMethodsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("process")]
        [RequireRole(UserRole.User)]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDTO request)
        {
            var command = new ProcessPaymentCommand
            {
                BookingId = request.BookingId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaymentDetails = request.PaymentDetails
            };

            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
} 