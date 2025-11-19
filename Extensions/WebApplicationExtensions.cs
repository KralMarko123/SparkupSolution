using SparkUpSolution.Infrastructure.Persistence;

namespace SparkUpSolution.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void WithDatabaseReset(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                appDbContext.Database.EnsureDeleted();
                appDbContext.Database.EnsureCreated();
            }
        }
    }
}
