// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddGraphQlClient().ConfigureHttpClient(client =>
{
  client.  
})





Console.WriteLine("Hello, World!");
Console.ReadLine();