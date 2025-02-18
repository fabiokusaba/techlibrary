using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

// Contém toda a regra de negócio para registrar um novo usuário
public class RegisterUserUseCase
{
    public UserRegisteredResponse Execute(UserRequest request)
    {
        // Validando os dados da requisição
        Validate(request);
        
        // Criando a entidade
        var entity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
        };
        
        // Instânciando o banco de dados
        var dbContext = new TechLibraryDbContext();
        
        // Acessando a tabela e inserindo um registro -> preparando a query
        dbContext.Users.Add(entity);
        // Executando a query
        dbContext.SaveChanges();

        return new UserRegisteredResponse
        {
            Name = entity.Name,
        };
    }
    
    private void Validate(UserRequest request)
    {
        var validator = new RegisterUserValidator();
        
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            
            // Lançando uma exceção, pois a nossa requisição não é válida
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}