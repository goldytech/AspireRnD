using Aspirant.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo");
var mongoDb = mongo.AddDatabase("graphql-db");

var graphqlApi = builder.AddProject<GraphQLApi>("graphql-api")
    .WithReference(mongoDb)
    .WaitFor(mongoDb);
builder.Build().Run();