using GraphQLApi.Utils;

namespace GraphQLApi.Data;

public class DatabaseSeederHostedService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseSeederHostedService>>();
        try
        {
            await seeder.SeedData();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}