using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Domain.Companies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReceivablesFactoring.Infrastructure.Authentication;

public class TokenProvider : ITokenProvider
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string CreateToken(Company company)
    {
        var chaveToken = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]!));

        var token = new JwtSecurityToken(
            claims: [new(ClaimTypes.NameIdentifier, company.Id.ToString())],
            expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpirationInMinutes"]!)),
            signingCredentials: new SigningCredentials(chaveToken, SecurityAlgorithms.HmacSha256),
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"]
        );

        return $"Bearer {new JwtSecurityTokenHandler().WriteToken(token)}";
    }

    public Guid GetCompanyIdFromToken() => Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
}
