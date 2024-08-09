using GraphQLDotNetClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
        var startup = new Startup();
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();
        _ = startup.Configure(app);

        app.Run();
    }
}