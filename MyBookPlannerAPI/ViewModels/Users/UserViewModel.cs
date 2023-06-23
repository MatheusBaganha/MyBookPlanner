using System.ComponentModel.DataAnnotations;

namespace MyBookPlannerAPI.ViewModels.Users
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Email needs to be a valid email adress.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
