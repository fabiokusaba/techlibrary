using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Users.Register;

// Criando um validator para a classe UserRequest
public class RegisterUserValidator : AbstractValidator<UserRequest>
{
    // Criando as regras
    public RegisterUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(request => request.Email).EmailAddress().WithMessage("O email não é válido.");
        RuleFor(request => request.Password).NotEmpty().WithMessage("A senha é obrigatória.");
        // Só quero executar a regra do número de caracteres se a senha não for vazia/nula
        When(request => string.IsNullOrEmpty(request.Password) == false, () =>
        {
            RuleFor(request => request.Password.Length).GreaterThanOrEqualTo(6)
                .WithMessage("A senha deve ser maior ou igual a 6.");
        });
    }
}