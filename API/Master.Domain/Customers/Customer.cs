using Master.Domain.Rules;
using Master.Domain.SeedWork;

namespace Master.Domain.Customers;

public class Customer : Entity, IAggregateRoot
{
    private Customer()
    {
    }

    private Customer(string email, string name)
    {
        Id = Guid.NewGuid();
        Email = email;
        Name = name;
        WelcomeEmailWasSent = false;
        AddDomainEvent(new CustomerRegisteredEvent(Id));
    }

    public Guid Id { get; }

    private string Email { get; set; }

    private string Name { get; set; }

    private bool WelcomeEmailWasSent { get; set; }

    public static Customer CreateRegistered(
        string email,
        string name)
    {
        CheckRule(new CustomerEmailMustBeUniqueRule(email));
        return new Customer(email, name);
    }

    public void MarkAsWelcomedByEmail()
    {
        WelcomeEmailWasSent = true;
    }
}