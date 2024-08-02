namespace GraphQLDotNetClient;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class TokenHttpClient(HttpClient httpClient)
{
    public async Task<string?> GetJwtTokenAsync(string username, string password)
    {
        var tokenRequest = new TokenRequestDto
        {
            Username = username,
            Password = password
        };

        var response = await httpClient.PostAsJsonAsync("/api/v1/auth/token", tokenRequest);
        response.EnsureSuccessStatusCode();

        var token = await response.Content.ReadFromJsonAsync<string>();
        return token;
    }
}

public class TokenRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}