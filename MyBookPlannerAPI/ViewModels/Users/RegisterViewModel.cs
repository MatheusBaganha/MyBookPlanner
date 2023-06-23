using System.ComponentModel.DataAnnotations;

namespace MyBookPlannerAPI.ViewModels.Users
{
    public class RegisterViewModel : UserViewModel
    {
        [Required]
        public string Username { get; set; }
    }
}
