using Microsoft.EntityFrameworkCore;

namespace Chat.API.Infrastructure.DatabaseInitialization
{
    /// <summary>
    /// Database Initializer
    /// </summary>
    public static class DatabaseInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context!.Database.MigrateAsync();

        }
    }
}