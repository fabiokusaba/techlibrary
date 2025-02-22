using System.Net;

namespace TechLibrary.Exception;

// Exceção customizada
public class ErrorOnValidationException : TechLibraryException
{
    // Somente essa classe vai ter acesso a essa lista de errors
    // readonly: somente o construtor pode atribuir valores
    private readonly List<string> _errors;
    
    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        _errors = errorMessages;    
    }
    
    public override List<string> GetErrorMessages() => _errors;
    
    // Erro 400: a requisição contém problemas
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}