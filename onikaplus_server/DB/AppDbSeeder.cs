using Microsoft.EntityFrameworkCore;
using onikaplus_server.Interfaces;

namespace onikaplus_server.DB;

public class AppDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }
    }
}