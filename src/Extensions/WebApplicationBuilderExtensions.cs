using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SparkUpSolution.Application.Helpers;
using SparkUpSolution.Application.Services;
using SparkUpSolution.Infrastructure.Logging;
using SparkUpSolution.Infrastructure.Repositories;
using SparkUpSolution.Middlewares;
using Swashbuckle.AspNetCore.Swagger;
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

        public static void WithAppServices(this WebApplicationBuilder builder)
        {
            // Repositories
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IBonusRepository, BonusRepository>();
            builder.Services.AddScoped<IBonusAuditRepository, BonusAuditRepository>();    

            // Services
            builder.Services.AddScoped<IBonusService, BonusService>();
            builder.Services.AddScoped<ICurrentOperatorAccessor, CurrentOperatorAccessor>();

            // Helpers
            builder.Services.AddScoped<IJwtHelper, JwtHelper>();

            // Middlewares
            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
        }

        public static void WithSwaggerEnabled(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sparkup API",
                    Version = "v1"
                });

                // Add the “Authorize” button
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: **Bearer {your token}**"
                });

                // Apply bearer to all operations
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        }, Array.Empty<string>()
                    }
                });

                // Add xml descriptions
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SparkUpSolution.xml"));
            });
        }
    }
}