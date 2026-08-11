namespace CineBook.DTOs
{
    public class ScheduleResponse
    {
        public long Id { get; set; }
        public long MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string? MovieDescription { get; set; }
        public int MovieDuration { get; set; }
        public long TheaterId { get; set; }
        public string TheaterName { get; set; } = string.Empty;
        public string TheaterLocation { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public decimal Price { get; set; }
    }

}
