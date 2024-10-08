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
    .WithEnvironment("AUDIENCE", "https://localhost:7006")
    .WithEnvironment("ISSUER", "https://localhost:7006")
    .WithSwaggerUI();

var graphqlDotNetClient = builder.AddProject<GraphQLDotNetClient>("graphql-dotnet-client")
    .WithReference(graphqlApi)
    .WaitFor(graphqlApi);

var graphqlJsClient = builder.AddNpmApp("graphql-js-client", "../GraphQLJSClient","watch")
    .WithReference(graphqlApi)
    .WaitFor(graphqlApi)
    .WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");

var graphqlpyClient = builder.AddPythonProject("graphql-py-client", "../GraphQLPyClient","main.py")
    .WithReference(graphqlApi)
    .WaitFor(graphqlApi);

builder.Build().Run();