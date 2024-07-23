using GraphQLApi.Data;

namespace GraphQLApi.Utils;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.AddNpgsqlDbContext<GraphQlDbContext>("graphql-db");

        // Not recommended for production use
        applicationBuilder.Services.AddMigration<GraphQlDbContext, DataSeed>();
    }
}