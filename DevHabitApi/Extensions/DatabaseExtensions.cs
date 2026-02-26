using DevHabitApi.Database;
using DevHabitApi.Entities;
using Microsoft.AspNetCore.Identity;
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

    public static async Task SeedInitialDataAsync(this WebApplication application)
    {
        using IServiceScope scope = application.Services.CreateScope();
        RoleManager<IdentityRole> roleManager =
            scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            if (!await roleManager.RoleExistsAsync(Roles.Member))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Member));
            }
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            application.Logger.LogInformation("succesfully created roles. ");
        }
        catch (Exception ex)
        {
            application.Logger.LogError(ex, "An error occured while seeding initial data.");
            throw;
        }
    }
}
