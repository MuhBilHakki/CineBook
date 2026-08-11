using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBook.Models
{
    [Table("BOOKING_SEAT")]
    public class BookingSeat
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [ForeignKey(nameof(Booking))]
        [Column("BOOKING_ID")]
        public long BookingId { get; set; }
        public Booking? Booking { get; set; }

        [ForeignKey(nameof(Seat))]
        [Column("SEAT_ID")]
        public long SeatId { get; set; }
        public Seat? Seat { get; set; }
    }
}
