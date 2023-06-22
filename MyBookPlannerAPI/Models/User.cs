using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyBookPlanner.Models
{

    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        [Column("Username", TypeName = "VARCHAR")]
        public string Username { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("Email", TypeName = "VARCHAR")]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("PasswordHash", TypeName = "VARCHAR")]
        public string PasswordHash { get; set; }

        [MaxLength(300)]
        [Column("Biography", TypeName = "VARCHAR")]
        [AllowNull]
        public string Biography { get; set; }
    }
}