using MyBookPlanner.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MyBookPlanner.Domain.Models
{
    [Table("UserBooks")]
    [PrimaryKey("IdUser","IdBook")]

    public class UserBook
    {
        [ForeignKey("IdUser")]
        [Column("IdUser", TypeName = "INT", Order = 1)]
        public int IdUser { get; set; }

        [ForeignKey("IdBook")]
        [Column("IdBook", TypeName = "INT", Order = 2)]
        public int IdBook { get; set; }


        [Required]
        [Column("UserScore", TypeName = "DECIMAL")]
        [Range(0.0, 10.0)]
        public float UserScore { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("ReadingStatus", TypeName = "VARCHAR")]
        public string ReadingStatus { get; set; }
    }
}
