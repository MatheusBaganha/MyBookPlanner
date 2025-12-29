using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBookPlanner.Domain.ViewModels.Users
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
