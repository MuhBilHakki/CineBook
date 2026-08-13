using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBook.Models
{
    [Table("BOOKING")]
    public class Booking
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [ForeignKey(nameof(User))]
        [Column("USER_ID")]
        public long UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey(nameof(Schedule))]
        [Column("SCHEDULE_ID")]
        public long ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }

        [Column("TOTAL_TICKETS")]
        public int TotalTickets { get; set; }

        [Column("TOTAL_PRICE", TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        [Column("STATUS")] 
        public BookingStatus Status { get; set; } = BookingStatus.PENDING;

        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
