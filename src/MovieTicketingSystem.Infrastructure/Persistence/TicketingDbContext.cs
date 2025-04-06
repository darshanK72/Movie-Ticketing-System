using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTicketingSystem.Domain.Entities;
using MovieTicketingSystem.Domain.Enums;

namespace MovieTicketingSystem.Infrastructure.Persistence;

public class TicketingDbContext : IdentityDbContext<User>
{
    public TicketingDbContext(DbContextOptions<TicketingDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Show> Shows { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Theater> Theaters { get; set; }
    public DbSet<CinemaHall> CinemaHalls { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<ShowSeat> ShowSeats { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Language> Languages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Director).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PosterUrl).HasMaxLength(500);
            entity.Property(e => e.TrailerUrl).HasMaxLength(500);
            entity.Property(e => e.ReleaseDate).IsRequired();
            entity.Property(e => e.CertificateRating).IsRequired();
            entity.Property(e => e.ViewerRating).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.TotalSeats).IsRequired();
            entity.Property(e => e.AvailableSeats).IsRequired();
            entity.Property(e => e.BasePrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.Movie)
                .WithMany(m => m.Shows)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.CinemaHall)
                .WithMany(ch => ch.Shows)
                .HasForeignKey(e => e.CinemaHallId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.ShowManager)
                .WithMany(u => u.ManagedShows)
                .HasForeignKey(e => e.ShowManagerId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NumberOfTickets).IsRequired();
            entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.BookingStatus).IsRequired();
            entity.Property(e => e.PaymentStatus).IsRequired();
            entity.Property(e => e.BookingDate).IsRequired();
            entity.Property(e => e.ExpirationTime).IsRequired();
            entity.Property(e => e.CancellationReason).HasMaxLength(500);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Show)
                .WithMany(s => s.Bookings)
                .HasForeignKey(e => e.ShowId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TransactionId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PaymentStatus).IsRequired();
            entity.Property(e => e.PaymentDate).IsRequired();
            entity.Property(e => e.RefundReason).HasMaxLength(500);
            entity.Property(e => e.FailureReason).HasMaxLength(500);

            entity.HasOne(e => e.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Website).HasMaxLength(200);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.Address)
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<CinemaHall>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.TotalSeats).IsRequired();
            entity.Property(e => e.NumberOfRows).IsRequired();
            entity.Property(e => e.SeatsPerRow).IsRequired();
            entity.Property(e => e.Has3D).IsRequired();
            entity.Property(e => e.HasDolby).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.Theater)
                .WithMany(t => t.CinemaHalls)
                .HasForeignKey(e => e.TheaterId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SeatNumber).IsRequired().HasMaxLength(10);
            entity.Property(e => e.RowNumber).IsRequired();
            entity.Property(e => e.ColumnNumber).IsRequired();
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.PriceMultiplier).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.IsActive).IsRequired();

            entity.HasOne(e => e.CinemaHall)
                .WithMany(ch => ch.Seats)
                .HasForeignKey(e => e.CinemaHallId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(e => e.ShowSeats)
                .WithOne(ss => ss.Seat)
                .HasForeignKey(ss => ss.SeatId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<ShowSeat>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsBooked).IsRequired();
            entity.Property(e => e.BookingStatus).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.Show)
                .WithMany(s => s.ShowSeats)
                .HasForeignKey(e => e.ShowId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Seat)
                .WithMany(s => s.ShowSeats)
                .HasForeignKey(e => e.SeatId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.Booking)
                .WithMany(b => b.ShowSeats)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.State).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PostalCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Details).HasMaxLength(500);
        });

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("MovieGenres"));

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Languages)
            .WithMany(l => l.Movies)
            .UsingEntity(j => j.ToTable("MovieLanguages"));
    }
}
