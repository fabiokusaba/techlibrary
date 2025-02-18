using Microsoft.AspNetCore.Mvc;
using TechLibrary.Api.UseCases.Users.Register;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Controllers;

[Route("v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(UserRegisteredResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorMessagesResponse), StatusCodes.Status400BadRequest)]
    public IActionResult Register(UserRequest request)
    {
        // Dentro do try estou tentando executar algo
        try
        {
            // Criando a instância do use case
            var useCase = new RegisterUserUseCase();

            // Executando a regra de negócio para criação de um usuário
            var response = useCase.Execute(request);

            return Created(string.Empty, response);
        }
        // Se alguma exceção ocorrer vou capturá-la no catch
        catch (TechLibraryException ex)
        {
            return BadRequest(new ErrorMessagesResponse
            {
                Errors = ex.GetErrorMessages()
            });
        }
        // Qualquer outro tipo de error
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorMessagesResponse
            {
                Errors = ["Erro Desconhecido."]
            });
        }
    }
}

