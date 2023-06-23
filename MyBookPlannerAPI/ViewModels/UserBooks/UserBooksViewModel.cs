using MyBookPlanner.Models;

namespace MyBookPlannerAPI.ViewModels.UserBooks
{
    public class UserBooksViewModel : Book
    {
        public int IdUser { get; set; }

        public int IdBook { get; set; }

        public float UserScore { get; set; }

        public string ReadingStatus { get; set; }
    }
}
