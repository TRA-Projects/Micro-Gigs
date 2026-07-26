using Micro_Gigs.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Micro_Gigs.Services
{
    public class AuthService
    {
        private IConfiguration config;

        // Constructor Injection - appsettings.json
        public AuthService(IConfiguration _config)
        {
            config = _config;
        }

        // Generate JWT Token
        public string GenerateToken(Users user)
        {
            // Get JWT settings from appsettings.json
            string secretKey = config["JwtSettings:SecretKey"]!;
            string issuer = config["JwtSettings:Issuer"]!;
            string audience = config["JwtSettings:Audience"]!;
            int hours = int.Parse(config["JwtSettings:ExpiryHours"]!);


            // Create Security Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            );


            // Signing Credentials
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );


            // Claims - User information stored inside JWT
            Claim[] claims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Role, user.UserType)
            };


            // Create Token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(hours),
                signingCredentials: credentials
            );


            // Convert Token Object to String
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
