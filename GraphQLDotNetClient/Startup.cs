using Demo.GraphqlDotNetClient;

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
        services.AddTransient<GraphQlService>(); // Register GraphQlService
        services.AddTransient<AuthTokenHandler>();

        #region Strawberry Shake HttpClient Configuration

        services.AddHttpClient(GraphQlClient.ClientName, client =>
        {
            client.BaseAddress = new Uri("https://graphql-api/graphql");
            client.Timeout = TimeSpan.FromMinutes(30); // This is done just for debug purposes.
        }).AddHttpMessageHandler<AuthTokenHandler>();
        services.AddGraphQlClient();

        #endregion


        #region Normal HttpClient Configuration
        services.AddHttpClient<GraphqlHttpClient>(client =>
        {
            client.Timeout = TimeSpan.FromMinutes(30); // This is done just for debug purposes.
            client.BaseAddress = new Uri("https://graphql-api");
        }).AddHttpMessageHandler<AuthTokenHandler>();
        #endregion
    }
        
    public async Task Configure(IApplicationBuilder app)
    {
        var graphQlService = app.ApplicationServices.GetRequiredService<GraphQlService>();
        
        await graphQlService.ExecuteGraphQlQuery();
        
        var graphqlHttpClient = app.ApplicationServices.GetRequiredService<GraphqlHttpClient>();
        var response = await graphqlHttpClient.FetchAlphaBroderProductsAsync();
        Console.WriteLine("GraphQL Query Executed via normal HttpClient");
        foreach (var node in response?.Data?.AlphaBroderProducts.Nodes!)
        {
            Console.WriteLine($"- {node.Name}");
        }
    }
}