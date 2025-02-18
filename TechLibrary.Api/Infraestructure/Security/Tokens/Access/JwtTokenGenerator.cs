using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infraestructure.Security.Tokens.Access;

public class JwtTokenGenerator
{
    public string Generate(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };
        
        // Descrevendo um token: falar para ele quando ele vai expirar, qual a credencial de acesso,
        // O token é composto por três partes: cabeçalho (header), corpo do token (payload) onde adicionamos informações
        // que vão estar presentes nele, assinatura (signature) partes um e dois combinadas e criptografadas
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // UtcNow: horário base que a partir dele é calculado as time zones, os horários para outros países e regiões
            Expires = DateTime.UtcNow.AddMinutes(60), // Validade de 1h a partir do horário de agora
            Subject = new ClaimsIdentity(claims), // Passar as identidades
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };
        
        // Gerando o token
        var tokenHandler = new JwtSecurityTokenHandler();
        
        // Criando um token com a descrição que passamos
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        
        // Com a função de escrever um token devolvemos um texto
        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var signingKey = "BJTCmimxU1cTPtHw3z32Csx4426zuyiS";
        
        var symmetricKey = Encoding.UTF8.GetBytes(signingKey);
        
        return new SymmetricSecurityKey(symmetricKey);
    }
}