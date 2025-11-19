using Microsoft.IdentityModel.Tokens;
using SparkUpSolution.Application.Helpers;
using SparkUpSolution.Application.Services;
using SparkUpSolution.Infrastructure.Repositories;
using System.Text;

namespace SparkUpSolution.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void WithAuthentication(this WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetSection("Jwt");

            builder.Services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
                    };
                });
        }

        public static void WithServices(this WebApplicationBuilder builder)
        {
            // Repos
            builder.Services.AddScoped<IBonusRepository, BonusRepository>();

            // Services
            builder.Services.AddScoped<IBonusService, BonusService>();

            // Helpers
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();
        }
    }
}
