using System.ComponentModel.DataAnnotations;

namespace CineBook.DTOs
{
    public class CreateBookingRequest
    {
        [Required(ErrorMessage = "ScheduleId wajib diisi")]
        public long ScheduleId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Pilih minimal 1 kursi")]
        public List<long> SeatIds { get; set; } = new();

        [Range(1, 10, ErrorMessage = "Total tiket antara 1 - 10")]
        public int TotalTickets { get; set; }

        public decimal TotalPrice { get; set; }
    }

}
