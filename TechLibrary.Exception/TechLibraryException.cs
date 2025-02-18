using System.Net;

namespace TechLibrary.Exception;

// Classe para identificarmos se é uma exceção do projeto ou não
public abstract class TechLibraryException : SystemException
{
    // Conceito de abstração: obrigando todas as classes filhas a implementarem duas funções
    public abstract List<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}