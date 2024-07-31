using System.Reflection;
using System.Text;
using GraphQLApi.Data;
using GraphQLApi.Queries;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Asp.Versioning;
using GraphQLApi.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

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
    public static void AddGraphQlServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddGraphQLServer()
            .AddAuthorization() // Register the GraphQL authorization directive
            .AddQueryType<EShopQueries>()
            .AddGlobalObjectIdentification()
            .AddMongoDbFiltering()
            .AddMongoDbSorting()
            .AddMongoDbProjections()
            .AddMongoDbPagingProviders();
    }
    
    public static void MapGroupEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }
    }
    
    public static void AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

    }
    
   public static void AddSwaggerSupport(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddSwaggerGen();
        webApplicationBuilder.Services.AddEndpointsApiExplorer();
        webApplicationBuilder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void AddAuthenticationServices(this WebApplicationBuilder webApplicationBuilder)
    {
        var issuer = Environment.GetEnvironmentVariable("ISSUER");
        var audience = Environment.GetEnvironmentVariable("AUDIENCE");
        var signingKey = Environment.GetEnvironmentVariable("SIGNING_KEY");

        webApplicationBuilder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!))
                };
            });
    }

    public static void AddAuthorizationServices(this WebApplicationBuilder webApplicationBuilder)
    {
        var allowedAlphaTenantIds = new List<string> { "123", "789" };
        webApplicationBuilder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AlphaTenantsPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context =>
                {
                    var tenantIdClaim = context.User.FindFirst("TenantId")?.Value;
                    return tenantIdClaim is not null && allowedAlphaTenantIds.Contains(tenantIdClaim);
                });
            });
        } );
    }
}