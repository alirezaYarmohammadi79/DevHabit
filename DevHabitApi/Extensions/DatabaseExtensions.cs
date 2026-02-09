using DevHabitApi.Database;
using Microsoft.EntityFrameworkCore;

namespace DevHabitApi.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

        try
        {
            await dbContext!.Database.MigrateAsync();

            app.Logger.LogInformation("Database Migrations applied successfully");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "an error occured while applying database migrations.");
            throw;
        }
    }
}
