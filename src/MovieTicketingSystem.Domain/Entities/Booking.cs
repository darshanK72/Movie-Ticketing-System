using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Entities
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public Guid ShowId { get; set; }

        [ForeignKey("ShowId")]
        public Show? Show { get; set; }

        public ICollection<ShowSeat>? ShowSeats { get; set; }

        [Required]
        public int NumberOfTickets { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public DateTime ExpirationTime { get; set; }

        public DateTime? CancellationDate { get; set; }

        [StringLength(500)]
        public string? CancellationReason { get; set; }

        public ICollection<Payment>? Payments { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 