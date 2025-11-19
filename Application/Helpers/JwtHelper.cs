using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SparkUpSolution.Application.Helpers
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IConfiguration configuration;
        
        public JwtHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> GenerateJwtToken(string username)
        {
            var jwtConfig = configuration.GetSection("Jwt");

            var claims = new[]
            {
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim("name", username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
