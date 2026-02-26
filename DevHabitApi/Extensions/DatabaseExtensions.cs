using DevHabitApi.Database;
using Microsoft.EntityFrameworkCore;

namespace DevHabitApi.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        await using ApplicationDbContext applicationContext = 
            scope.ServiceProvider.GetService<ApplicationDbContext>();

        await using ApplicationIdentityDbContext identityContext = 
            scope.ServiceProvider.GetService<ApplicationIdentityDbContext>();

        try
        {
            await applicationContext!.Database.MigrateAsync();
            app.Logger.LogInformation("Application database Migrations applied successfully");

            await identityContext!.Database.MigrateAsync();
            app.Logger.LogInformation("Identity database Migrations applied successfully");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "an error occured while applying database migrations.");
            throw;
        }
    }
}
