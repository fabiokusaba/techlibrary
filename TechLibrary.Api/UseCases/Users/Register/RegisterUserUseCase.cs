using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

// Contém toda a regra de negócio para registrar um novo usuário
public class RegisterUserUseCase
{
    public UserRegisteredResponse Execute(UserRequest request)
    {
        // Instânciando o banco de dados
        var dbContext = new TechLibraryDbContext();
        
        // Validando os dados da requisição
        Validate(request, dbContext);
        
        // Instânciando BCrypt
        var cryptography = new BCryptAlgorithm();
        
        // Criando a entidade
        var entity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = cryptography.HashPassword(request.Password), // Criptografando a senha do usuário
        };
        
        // Acessando a tabela e inserindo um registro -> preparando a query
        dbContext.Users.Add(entity);
        // Executando a query
        dbContext.SaveChanges();

        // Instânciando o token generator
        var tokenGenerator = new JwtTokenGenerator();

        return new UserRegisteredResponse
        {
            Name = entity.Name,
            AccessToken = tokenGenerator.Generate(entity) // Gerando um token para o usuário
        };
    }
    
    private void Validate(UserRequest request, TechLibraryDbContext dbContext)
    {
        var validator = new RegisterUserValidator();
        
        var result = validator.Validate(request);
        
        // Validando emails duplicados
        // Na tabela Users estou verificando se existe qualquer usuário com o mesmo email que recebi da request
        var existUserWithEmail = dbContext.Users.Any(user => user.Email.Equals(request.Email));
        if (existUserWithEmail)
            result.Errors.Add(new ValidationFailure("Email", "Email já registrado na plataforma."));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            
            // Lançando uma exceção, pois a nossa requisição não é válida
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}