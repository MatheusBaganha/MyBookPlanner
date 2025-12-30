using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBookPlanner.Domain.DTO
{
    public class RegisterUserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
