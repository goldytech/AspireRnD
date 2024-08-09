namespace GraphQLDotNetClient;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class GraphqlHttpClient(HttpClient httpClient)
{
    public async Task<GraphQLResponse?> FetchAlphaBroderProductsAsync()
    {
        var query = new
        {
            query = """
                    query alphaborder {
                                    alphaBroderProducts {
                                        nodes {
                                            alphaBroderCategory {
                                                description
                                                id
                                                lastModifiedAt
                                                name
                                            }
                                            description
                                            name
                                            price
                                        }
                                    }
                                }
                    """
        };

        var content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("/graphql", content);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var graphQlResponse = JsonSerializer.Deserialize<GraphQLResponse>(responseString, options);

        if (graphQlResponse?.Data == null)
        {
            Console.WriteLine("Deserialization failed or no data in response.");
        }

        return graphQlResponse;
    }
}