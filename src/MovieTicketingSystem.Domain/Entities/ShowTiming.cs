using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Entities
{
    [Table("ShowTimings")]
    public class ShowTiming
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid ShowId { get; set; }

        [ForeignKey("ShowId")]
        public Show? Show { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        public int AvailableSeats { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        [Required]
        public ShowStatus ShowStatus { get; set; }

        [Required]
        public string? ShowManagerId { get; set; }

        [ForeignKey("ShowManagerId")]
        public User? ShowManager { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        public ICollection<ShowSeat>? ShowSeats { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 