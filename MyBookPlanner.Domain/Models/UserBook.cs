namespace MyBookPlanner.Domain.Models
{
    public class UserBook
    {
        public int IdUser { get; set; }
        public int IdBook { get; set; }
        public float UserScore { get; set; }
        public string ReadingStatus { get; set; }

        public Book Book { get; set; }
        public User User { get; set; }
    }
}
