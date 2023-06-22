using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBookPlanner.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MyBookPlannerAPI.Models
{
    public class UserBook
    {
        [Required]
        [ForeignKey("IdUser")]
        [Column("IdUser", TypeName = "INT")]
        public int IdUser { get; set; }

        public User User { get; set; }

        [Required]
        [ForeignKey("IdBook")]
        [Column("IdBook", TypeName = "INT")]
        public int IdBook { get; set; }

        public Book Book {  get; set; }

        [Required]
        [Column("UserScore", TypeName = "FLOAT")]
        public float UserScore { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("ReadingStatus", TypeName = "VARCHAR")]
        public string ReadingStatus { get; set; }

    }
}
    //IdUser INT,
    //IdBook INT,
    //UserScore FLOAT,
    //ReadingStatus VARCHAR(50)

    //CONSTRAINT FK_IdUser FOREIGN KEY (IdUser) REFERENCES[Users](Id),
    //CONSTRAINT FK_IdBook FOREIGN KEY (IdBook) REFERENCES[Books](Id),