using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineBook.Models
{
    [Table("USER")]
    public class User
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        [Column("EMAIL")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)] 
        [Column("PASSWORD")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("NAMA")]
        public string Nama { get; set; } = string.Empty;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
