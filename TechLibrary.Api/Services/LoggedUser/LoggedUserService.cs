using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TechLibrary.Api.Services.LoggedUser;

public class LoggedUserService
{
    private readonly HttpContext _httpContext;
    
    public LoggedUserService(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public User GetLoggedUser(TechLibraryDbContext dbContext)
    {
        var authentication = _httpContext.Request.Headers.Authorization.ToString();

        // Pegar um token
        var token = authentication["Bearer".Length..].Trim();

        // Criar um token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // Ler o token
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        // Nas claims do token pega a primeira claim que o tipo dela é Sub (Id do usuário)
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
        
        // Convertendo a string para um Guid
        var userId = Guid.Parse(identifier);
        
        // Ir ao banco de dados para pegar o usuário com esse id
        return dbContext.Users.First(user => user.Id == userId);
    }
}