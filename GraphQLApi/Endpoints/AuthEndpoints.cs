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
                case { Username: "t1", Password: "secret" }:
                {
                    // Generate a JWT Token and return it
                    var token = GenerateJwtToken(tokenRequestDto.Username);
                    return Results.Ok(token);
                }
                case {Username:"t2", Password:"secret"}:
                {
                    var token = GenerateJwtToken(tokenRequestDto.Username);
                    return Results.Ok(token);
                }
                case {Username:"t3", Password:"secret"}:
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
            "t1" => 123,
            "t2" => 456,
            "t3" => 789,
            _ => 0
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("TenantId", tenantId.ToString())
            }),
            Audience = Environment.GetEnvironmentVariable("AUDIENCE"), //Intended Recipient of the Token
            Issuer = Environment.GetEnvironmentVariable("ISSUER"), // Issuer of the Token
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