using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GraphQLApi.Utils;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLApi.Endpoints;

public class AuthEndpoints :IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/token", (TokenRequestDto tokenRequestDto) =>
        {
            switch (tokenRequestDto)
            {
                // Validations Omitted
                case { Username: "alpha", Password: "alpha" }:
                {
                    // Generate a JWT Token and return it
                    var token = GenerateJwtToken(tokenRequestDto.Username);
                    return Results.Ok(token);
                }
                case {Username:"sanmar", Password:"sanmar"}:
                {
                    var token = GenerateJwtToken(tokenRequestDto.Username);
                    return Results.Ok(token);
                }
                default:
                    return Results.Unauthorized();
            }
        });
    }
    
    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SIGNING_KEY")!.ToArray());
        var tenantId = username switch
        {
            "alpha" => 123,
            "sanmar" => 456,
            _ => 0
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Claims = new Dictionary<string, object>
            {
                {"TenantId", tenantId}
            },
            
            
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
       
    }
}


public record TokenRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}