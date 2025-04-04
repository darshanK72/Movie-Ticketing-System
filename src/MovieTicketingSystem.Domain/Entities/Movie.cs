using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Domain.Entities
{
    [Table("Movies")]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string? Genre { get; set; }

        [Required]
        [StringLength(20)]
        public string? Language { get; set; }

        [Required]
        public int DurationInMinutes { get; set; }

        [Required]
        [StringLength(100)]
        public string? Director { get; set; }

        [Required]
        [StringLength(500)]
        public string? Cast { get; set; }

        [StringLength(500)]
        public string? PosterUrl { get; set; }

        [StringLength(500)]
        public string? TrailerUrl { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public MovieRating Rating { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Show>? Shows { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
} 