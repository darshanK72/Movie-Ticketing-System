using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Entities
{
    [Table("ShowSeats")]
    public class ShowSeat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid ShowTimingId { get; set; }

        [ForeignKey("ShowTimingId")]
        public ShowTiming? ShowTiming { get; set; }

        [Required]
        public Guid SeatId { get; set; }

        [ForeignKey("SeatId")]
        public Seat? Seat { get; set; }

        [Required]
        public bool IsBooked { get; set; }

        public Guid? BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking? Booking { get; set; }

        [Required]
        public SeatBookingStatus SeatBookingStatus { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 