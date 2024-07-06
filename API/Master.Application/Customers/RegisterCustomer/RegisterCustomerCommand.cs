using Master.Application.DTOs;
using Master.Infrastructure.Commands;

namespace Master.Application.Customers.RegisterCustomer;

public class RegisterCustomerCommand : CommandBase<CustomerDTO>
{
    public RegisterCustomerCommand(string email, string name)
    {
        Email = email;
        Name = name;
    }

    public string Email { get; }

    public string Name { get; }
}