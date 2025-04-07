using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketingSystem.Application.Commands.Bookings;
using MovieTicketingSystem.Application.Queries.Bookings;
using MovieTicketingSystem.Domain.DTOs;
using MovieTicketingSystem.Domain.Enums;
using MovieTicketingSystem.Infrastructure.Authorization;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [RequireRole(UserRole.ShowManager)]
        public async Task<IActionResult> GetAllBookings()
        {
            var query = new GetAllBookingsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [RequireRole(UserRole.User)]
        public async Task<IActionResult> GetBookingById(string id)
        {
            var query = new GetBookingByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound(new { Message = "Booking Not Found" });

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        [RequireRole(UserRole.User)]
        public async Task<IActionResult> GetUserBookings(string userId)
        {
            var query = new GetUserBookingsQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [RequireRole(UserRole.User)]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var bookingId = await _mediator.Send(command);
            if (string.IsNullOrEmpty(bookingId))
                return BadRequest(new { Message = "Failed to create booking" });

            return Ok(new { BookingId = bookingId, Message = "Booking created successfully. Please complete payment within 5 minutes." });
        }

        [HttpPost("{id}/cancel")]
        [RequireRole(UserRole.User)]
        public async Task<IActionResult> CancelBooking(string id, [FromBody] string reason)
        {
            var command = new CancelBookingCommand
            {
                BookingId = id,
                CancellationReason = reason
            };

            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest(new { Message = "Failed to cancel booking" });

            return Ok(new { Message = "Booking cancelled successfully" });
        }

        [HttpPost("cancel-expired")]
        [RequireRole(UserRole.User)]
        public async Task<IActionResult> CancelExpiredBooking()
        {
            var command = new CancleExpiredBookingsCommand();

            var result = await _mediator.Send(command);
            if (!result)
                return BadRequest(new { Message = "Failed to cancel bookings" });

            return Ok(new { Message = "Expired Bookings cancelled successfully" });
        }

        [HttpPost("{id}/confirm")]
        [RequireRole(UserRole.User)]
        public async Task<IActionResult> ConfirmBooking(string id)
        {
            var command = new ConfirmBookingCommand { BookingId = id };
            var result = await _mediator.Send(command);
            
            if (!result)
                return BadRequest(new { Message = "Failed to confirm booking" });

            return Ok(new { Message = "Booking confirmed successfully" });
        }
    }
}
