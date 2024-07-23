using AppHost;
using Aspirant.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresQl = builder.AddPostgres("postgresQL")
    .WithHealthCheck().WithPgAdmin();
var postgres = postgresQl.AddDatabase("graphql-db");

var graphqlApi = builder.AddProject<GraphQLApi>("graphql-api")
    .WithReference(postgres)
    .WaitFor(postgres);
builder.Build().Run();