using TechLibrary.Api.Infraestructure;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin;

public class DoLoginUseCase
{
    public UserRegisteredResponse Execute(LoginRequest request)
    {
        // Instânciando o contexto
        var dbContext = new TechLibraryDbContext();

        // Buscando na tabela Users o usuário pelo seu email
        var user = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email));
        if (user is null)
            throw new InvalidLoginException();

        // Verificando as senhas
        var cryptography = new BCryptAlgorithm();
        var passwordIsValid = cryptography.Verify(request.Password, user);
        if (passwordIsValid == false)
            throw new InvalidLoginException();
        
        // Instânciando o token generator
        var tokenGenerator = new JwtTokenGenerator();
        
        return new UserRegisteredResponse
        {
            Name = user.Name,
            AccessToken = tokenGenerator.Generate(user)
        };
    }
}