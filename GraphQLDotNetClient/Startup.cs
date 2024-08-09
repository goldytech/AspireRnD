namespace GraphQLDotNetClient;

using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient<TokenHttpClient>(client =>
        {
            client.BaseAddress = new Uri("https://graphql-api");
        });

        services.AddTransient<AuthTokenHandler>();
        
        services.AddGraphQlClient().ConfigureHttpClient(async (provider, httpClient) =>
        {
            httpClient.BaseAddress = new Uri("https://graphql-api/graphql");
            var tokenHttpClient = provider.GetRequiredService<TokenHttpClient>();
            var token = await tokenHttpClient.GetJwtTokenAsync("t1", "secret");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine("Token: " + token);
        });
        services.AddTransient<GraphQlService>(); // Register GraphQlService

        services.AddHttpClient<GraphqlHttpClient>(client =>
        {
            client.Timeout = TimeSpan.FromMinutes(30);
            client.BaseAddress = new Uri("https://graphql-api");
        }).AddHttpMessageHandler<AuthTokenHandler>();
    }

    public async Task Configure(IApplicationBuilder app)
    {
        // var graphQlService = app.ApplicationServices.GetRequiredService<GraphQlService>();
        // graphQlService.ExecuteGraphQlQuery().GetAwaiter().GetResult();
        
        var graphqlHttpClient = app.ApplicationServices.GetRequiredService<GraphqlHttpClient>();
        var response = await graphqlHttpClient.FetchAlphaBroderProductsAsync();
        Console.WriteLine("GraphQL Query Executed");
        foreach (var node in response?.Data?.AlphaBroderProducts.Nodes!)
        {
            Console.WriteLine($"- {node.Name}");
        }
    }
}