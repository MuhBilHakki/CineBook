using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBook.Models
{
    [Table("THEATER")]
    public class Theater
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NAME")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        [Column("LOCATION")]
        public string Location { get; set; } = string.Empty;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
