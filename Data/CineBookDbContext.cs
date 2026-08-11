using CineBook.Models;
using Microsoft.EntityFrameworkCore;

namespace CineBook.Data
{
    public class CineBookDbContext : DbContext
    {
        public CineBookDbContext(DbContextOptions<CineBookDbContext> options)
        : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookingSeat> BookingsSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .Property(b => b.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Theater)
                .WithMany(t => t.Schedules)
                .HasForeignKey(s => s.TheaterId);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Movie)
                .WithMany(t => t.Schedules)
                .HasForeignKey(s => s.MovieId);

            modelBuilder.Entity<Seat>()
                .HasOne(se => se.Schedule)
                .WithMany(t => t.Seats)
                .HasForeignKey(se => se.ScheduleId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Schedule)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // PIVOT MANY TO MANY
            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId);

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Seat)
                .WithMany(b => b.BookingSeats)
                //.WithMany()
                .HasForeignKey(bs => bs.SeatId);
        }
    }

    
}
