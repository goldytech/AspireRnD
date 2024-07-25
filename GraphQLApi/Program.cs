using GraphQLApi;
using GraphQLApi.Utils;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddMongoDbServices();
builder.AddDataSeedingServices();
builder.AddGraphQlServices();

var app = builder.Build();

app.MapGraphQL();

app.Run();