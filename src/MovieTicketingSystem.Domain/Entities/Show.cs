using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Entities
{
    [Table("Shows")]
    public class Show
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }

        [Required]
        public Guid CinemaHallId { get; set; }

        [ForeignKey("CinemaHallId")]
        public CinemaHall? CinemaHall { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<ShowTiming>? ShowTimings { get; set; }
    }
} 