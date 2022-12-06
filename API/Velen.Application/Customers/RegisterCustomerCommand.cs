using FluentValidation;
using Velen.Application.Customers;
using Velen.Infrastructure.Commands;

namespace Velen.Application.Customers
{
    public class RegisterCustomerCommand : CommandBase<CustomerDto>
    {
        public string Email { get; }

        public string Name { get; }

        public RegisterCustomerCommand(string email, string name)
        {
            this.Email = email;
            this.Name = name;
        }      
    }
}
