
using DesafioVsoft.Api.Dtos;
using FluentValidation;

namespace DesafioVsoft.Api.Validators;

public class TaskAssignDtoValidator : AbstractValidator<TaskAssignDto>
{
    public TaskAssignDtoValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("TaskId é obrigatório");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId é obrigatório");
    }
}
