namespace GraphQLDotNetClient;

using System;
using System.Threading.Tasks;
using Demo.GraphqlDotNetClient;
using StrawberryShake;

public class GraphQlService
{
    private readonly IGraphQlClient _graphQlClient;
    
    public GraphQlService(IGraphQlClient graphQlClient)
    {
        _graphQlClient = graphQlClient;
    }
    
    public async Task ExecuteGraphQlQuery()
    {
        try
        {
            var result = await _graphQlClient.Alphaborder.ExecuteAsync();
            Console.WriteLine("GraphQL Query Executed via StrawberryShake");
            result.EnsureNoErrors();
            foreach (var resultError in result.Errors)
            {
                Console.WriteLine(resultError.Message);
            }
            foreach (var node in result.Data?.AlphaBroderProducts.Nodes!)
            {
                Console.WriteLine($"- {node.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}