using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyBookPlanner.Domain.Models;

namespace MyBookPlanner.WebApi.Configurations
{
    public static class JwtConfiguration
    {
        public static string JwtKey { get; set; }

        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Key of the token needs to be an array of byte.
            var key = Encoding.ASCII.GetBytes(JwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("username", user.Username),
                    new Claim("biography", user.Biography),
                }),
                Expires = DateTime.UtcNow.AddHours(8),

                // Defines how it will be encrypted and how it will be decrypted.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            // It creates our token based on the tokenDescriptor.
            var token = tokenHandler.CreateToken(tokenDescriptor);


            // It will return the token in string format.
            return tokenHandler.WriteToken(token);
        }
    }
}
