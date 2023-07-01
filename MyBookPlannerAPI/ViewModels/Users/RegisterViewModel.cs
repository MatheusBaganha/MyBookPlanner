using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBookPlannerAPI.ViewModels.Users
{
    public class RegisterViewModel : UserViewModel
    {
        [Required]
        public string Username { get; set; }
 
    }
}
