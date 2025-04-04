using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketingSystem.Domain.Entities
{
    [Table("CinemaHalls")]
    public class CinemaHall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        public int NumberOfRows { get; set; }

        [Required]
        public int SeatsPerRow { get; set; }

        [Required]
        public bool Has3D { get; set; }

        [Required]
        public bool HasDolby { get; set; }

        [Required]
        public Guid TheaterId { get; set; }

        [ForeignKey("TheaterId")]
        public Theater? Theater { get; set; }

        public ICollection<Seat>? Seats { get; set; }

        public ICollection<Show>? Shows { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 