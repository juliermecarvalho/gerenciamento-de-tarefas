
using DesafioVsoft.Api.Dtos;
using FluentValidation;

namespace DesafioVsoft.Api.Validators;

public class TaskInputDtoValidator : AbstractValidator<TaskInputDto>
{
    public TaskInputDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é obrigatório")
            .MaximumLength(100).WithMessage("Título deve ter no máximo 100 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória")
            .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres");


    }
}
