using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketingSystem.Application.Commands.Bookings;
using MovieTicketingSystem.Application.Queries.Bookings;
using MovieTicketingSystem.Domain.DTOs;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult GetAllBookings()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBookingById(string id)
        {
            var query = new GetBookingByIdQuery { BookingId = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<BookingDTO>>> GetUserBookings(string userId)
        {
            var query = new GetUserBookingsQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var bookingId = await _mediator.Send(command);
            if (string.IsNullOrEmpty(bookingId))
                return BadRequest("Failed to create booking");

            return Ok(new { BookingId = bookingId, Message = "Booking created successfully. Please complete payment within 5 minutes." });
        }

        [HttpPost("payment")]
        public async Task<ActionResult<bool>> MakePayment([FromBody] MakePaymentCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Failed to process payment");

            return Ok(new { Message = "Payment processed successfully. Your booking is confirmed." });
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<bool>> CancelBooking(string id, [FromBody] string reason)
        {
            var command = new CancelBookingCommand
            {
                BookingId = id,
                CancellationReason = reason
            };

            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest("Failed to cancel booking");

            return Ok(new { Message = "Booking cancelled successfully" });
        }

        [HttpPost("{id}/confirm")]
        public IActionResult ConfirmBooking(int id)
        {
            // Implementation will be added later
            return Ok();
        }
    }
}
