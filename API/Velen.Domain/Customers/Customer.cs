using Velen.Domain.SeedWork;

namespace Velen.Domain.Customers
{
    public class Customer : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }

        private string Email { get; set; }

        private string Name { get; set; }

        private bool WelcomeEmailWasSent { get; set; }

        private Customer()
        {
        }

        private Customer(string email, string name)
        {
            this.Id = Guid.NewGuid();
            Email = email;
            Name = name;
            WelcomeEmailWasSent = false;
            this.AddDomainEvent(new CustomerRegisteredEvent(this.Id));
        }

        public static Customer CreateRegistered(
            string email,
            string name)
        {
            return new Customer(email, name);
        }

        public void MarkAsWelcomedByEmail()
        {
            this.WelcomeEmailWasSent = true;
        }
    }
}