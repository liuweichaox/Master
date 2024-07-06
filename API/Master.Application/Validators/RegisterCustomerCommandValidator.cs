using FluentValidation;
using Master.Application.Commands;

namespace Master.Application.Validators;

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}