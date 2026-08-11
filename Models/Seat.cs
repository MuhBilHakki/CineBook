using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBook.Models
{
    [Table("SEAT")]
    public class Seat
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [ForeignKey(nameof(Schedule))]
        [Column("SCHEDULE_ID")]
        public long ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }

        [Required]
        [Column("SEAT_NUMBER")]
        public string SeatNumber { get; set; } = string.Empty;

        [Column("IS_AVAILABLE")]
        public bool IsAvailable { get; set; }

        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
