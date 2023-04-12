using FluentValidation;
using Velen.Application.Customers.RegisterCustomer;

namespace Velen.Application.Customers;

public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
{
    public RegisterCustomerCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}