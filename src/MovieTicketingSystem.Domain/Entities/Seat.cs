using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Entities
{
    [Table("Seats")]
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        public string? SeatNumber { get; set; }

        [Required]
        public int? RowNumber { get; set; }

        [Required]
        public int ColumnNumber { get; set; }

        [Required]
        public SeatType Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceMultiplier { get; set; }

        [Required]
        public Guid CinemaHallId { get; set; }

        [ForeignKey("CinemaHallId")]
        public CinemaHall? CinemaHall { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
} 