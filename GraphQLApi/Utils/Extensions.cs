using GraphQLApi.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace GraphQLApi.Utils;

public static class Extensions
{
    public static void AddMongoDbServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.AddMongoDBClient("graphql-db");
        var mongoDbConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__graphql-db");
        applicationBuilder.Services.AddSingleton(sp =>
        {
            var mongoConnectionUrl = new MongoUrl(mongoDbConnectionString);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            mongoClientSettings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    sp.GetRequiredService<ILogger<MongoDbLoggerCategory>>()
                        .LogMongoDbCommand(e.CommandName, e.Command.ToJson());
                });
            };
            var client = new MongoClient(mongoClientSettings);
            var database = client.GetDatabase(mongoConnectionUrl.DatabaseName);
            return database;
        });
    }
    
    public static void AddDataSeedingServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddSingleton<DatabaseSeeder>();
        applicationBuilder.Services.AddHostedService<DatabaseSeederHostedService>();
    }
}