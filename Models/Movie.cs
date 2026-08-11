using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBook.Models
{
    [Table("MOVIE")]
    public class Movie
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("TITLE")]
        public string Title { get; set; } = string.Empty;

        [Column("DESCRIPTION",TypeName = "text")]
        public string? Description { get; set; }
        [Column("DURATION")]
        public int Duration { get; set; } 

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
