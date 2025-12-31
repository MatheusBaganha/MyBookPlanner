using MyBookPlanner.Domain.Models;
using MyBookPlanner.Domain.ViewModels.Users;

namespace MyBookPlanner.Utils.Mappers
{
    public static class UserMapper
    {
        public static UserViewModel Convert(this User user)
        {
            return new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Biography = user.Biography,
            };
        } 
    }
}
