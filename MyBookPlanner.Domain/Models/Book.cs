using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MyBookPlanner.Domain.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
        public string ImageUrl { get; set; }
        public float Score { get; set; }

        public ICollection<UserBook> UserBooks { get; set; } = new List<UserBook>();
    }
}
