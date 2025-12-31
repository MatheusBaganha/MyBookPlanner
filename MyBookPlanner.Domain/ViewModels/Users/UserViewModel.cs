using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBookPlanner.Domain.ViewModels.Users
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Biography { get; set; }
        public string UserToken { get; set; }

    }
}
