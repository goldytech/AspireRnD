using Aspirant.Hosting;
using HealthChecks.NpgSql;

namespace AppHost;

public static class HealthCheckExtensions
{
    public static IResourceBuilder<PostgresServerResource> WithHealthCheck(
        this IResourceBuilder<PostgresServerResource> builder)
    {
        return builder.WithAnnotation(
            HealthCheckAnnotation.Create(cs => new NpgSqlHealthCheck(new NpgSqlHealthCheckOptions(cs))));
    }
}