using Aspirant.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo")
    .WithDataVolume("mongo-data");
    
var mongoDb = mongo.AddDatabase("graphql-db");

var graphqlApi = builder.AddProject<GraphQLApi>("graphql-api")
    .WithReference(mongoDb)
    .WaitFor(mongoDb)
    .WithEnvironment("SIGNING_KEY", "bXlTdXBlclNlY3VyZVNlY3JldEtleU5ldmVyU2hhcmU=")
    .WithEnvironment("AUDIENCE", "http://localhost:5000")
    .WithEnvironment("ISSUER", "https://localhost:7006")
    .WithSwaggerUI();

var graphqlDotNetClient = builder.AddProject<GraphQLDotNetClient>("graphql-dotnet-client")
    .WithReference(graphqlApi)
    .WaitFor(graphqlApi);

builder.Build().Run();