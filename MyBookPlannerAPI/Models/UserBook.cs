using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBookPlanner.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MyBookPlannerAPI.Models
{
    [Table("UserBooks")]
    [Keyless]
    public class UserBook
    {
        [Required]
        [ForeignKey("IdUser")]
        [Column("IdUser", TypeName = "INT")]
        public int IdUser { get; set; }

        [Required]
        [ForeignKey("IdBook")]
        [Column("IdBook", TypeName = "INT")]
        public int IdBook { get; set; }


        [Required]
        [Column("UserScore", TypeName = "FLOAT")]
        public float UserScore { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("ReadingStatus", TypeName = "VARCHAR")]
        public string ReadingStatus { get; set; }

    }
}
