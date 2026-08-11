using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBook.Models
{
    [Table("SCHEDULE")]
    public class Schedule
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [ForeignKey(nameof(Movie))]
        [Column("MOVIE_ID")]
        public long MovieId { get; set; }
        public Movie? Movie { get; set; }

        [ForeignKey(nameof(Theater))]
        [Column("THEATER_ID")]
        public long TheaterId { get; set; }
        public Theater? Theater { get; set; }

        [Column("START_TIME")]
        public DateTime StartTime { get; set; }

        [Column("PRICE",TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
