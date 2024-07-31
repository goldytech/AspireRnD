using Asp.Versioning;
using GraphQLApi;
using GraphQLApi.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddSwaggerSupport();
builder.AddMongoDbServices();
builder.AddDataSeedingServices();
builder.AddAuthenticationServices();
builder.AddAuthorizationServices();
builder.AddGraphQlServices();
builder.Services.AddEndpoints(typeof(Program).Assembly);
var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();
var authversionGroup = app.MapGroup("api/v{version:apiVersion}/auth").WithApiVersionSet(apiVersionSet);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapGroupEndpoints(authversionGroup);
app.MapGraphQL();

app.Run();