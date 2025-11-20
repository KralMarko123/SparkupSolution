using SparkUpSolution.Infrastructure.Persistence;

namespace SparkUpSolution.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task WithDatabaseReset(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                appDbContext.Database.EnsureDeleted();
                appDbContext.Database.EnsureCreated();

                if (app.Environment.IsDevelopment())
                {
                    await appDbContext.Seed();
                }
            }
        }
    }
}
