using System.ComponentModel.DataAnnotations;

namespace MyBookPlannerAPI.ViewModels.Users
{
    public class UpdateUserViewModel : UserViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username needs to have at least 3 characters, and the maximum of 100 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 0, ErrorMessage = "Biography needs to have less than 300 characters."),]
        public string Biography { get; set; }
    }
}
