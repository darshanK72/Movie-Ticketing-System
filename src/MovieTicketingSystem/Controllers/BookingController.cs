using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieTicketingSystem.Controllers
{
    [Route("api/bookings")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBookings()
        {
            // Implementation will be added later
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetBookingById(int id)
        {
            // Implementation will be added later
            return Ok();
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUserBookings(int userId)
        {
            // Implementation will be added later
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateBooking()
        {
            // Implementation will be added later
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id)
        {
            // Implementation will be added later
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult CancelBooking(int id)
        {
            // Implementation will be added later
            return Ok();
        }

        [HttpPost("{id}/confirm")]
        public IActionResult ConfirmBooking(int id)
        {
            // Implementation will be added later
            return Ok();
        }
    }
}
