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
        
        services.AddGraphQlClient().ConfigureHttpClient(async (provider, httpClient) =>
        {
            httpClient.BaseAddress = new Uri("https://graphql-api/graphql");
            var tokenHttpClient = provider.GetRequiredService<TokenHttpClient>();
            var token = await tokenHttpClient.GetJwtTokenAsync("t1", "secret");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine("Token: " + token);
        });
        services.AddTransient<GraphQlService>(); // Register GraphQlService
    }

    public void Configure(IApplicationBuilder app)
    {
        var graphQlService = app.ApplicationServices.GetRequiredService<GraphQlService>();
        graphQlService.ExecuteGraphQlQuery().GetAwaiter().GetResult();
    }
}