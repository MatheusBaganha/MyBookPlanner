using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyBookPlanner.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Title", TypeName = "VARCHAR")]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Author", TypeName = "VARCHAR")]
        public string Author { get; set; }

        [Required]
        [Column("ReleaseYear", TypeName = "INT")]
        public int ReleaseYear { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("ImageUrl", TypeName = "VARCHAR")]
        public string ImageUrl { get; set; }


        [Required]
        [Column("Score", TypeName = "FLOAT")]
        public float Score { get; set; }

        [AllowNull]
        [Column("UserScore", TypeName = "FLOAT")]
        public float UserScore { get; set; }

        [MaxLength(50)]
        [Column("ReadingStatus", TypeName = "VARCHAR")]
        public string ReadingStatus { get; set; }
    }
}
