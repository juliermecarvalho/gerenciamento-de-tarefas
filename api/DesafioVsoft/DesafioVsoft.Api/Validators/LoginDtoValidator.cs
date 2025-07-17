
using DesafioVsoft.Api.Dtos;
using FluentValidation;

namespace DesafioVsoft.Api.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(3).WithMessage("Senha deve conter ao menos 3 caracteres.");
    }
}
