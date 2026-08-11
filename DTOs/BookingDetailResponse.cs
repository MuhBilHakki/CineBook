namespace CineBook.DTOs
{
    public class BookingDetailResponse
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ScheduleId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheaterName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public int TotalTickets { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }

}
