using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.Filters;

// Fitro de exceção: toda exceção que acontecer no nosso código vou capturar ela aqui
public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Verificando o tipo da exceção
        if (context.Exception is TechLibraryException techLibraryException)
        {
            // Fazendo a conversão de um enum para um inteiro
            context.HttpContext.Response.StatusCode = (int)techLibraryException.GetStatusCode();
            context.Result = new ObjectResult(new ErrorMessagesResponse
            {
                Errors = techLibraryException.GetErrorMessages()
            });
        }
        else
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ErrorMessagesResponse
            {
                Errors = ["Erro Desconhecido."]
            });
        }
    }
}