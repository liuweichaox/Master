namespace Velen.Application.Customers.GetCustomerDetails
{
    public class CustomerDetailsDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string WelcomeEmailWasSent { get; set; }
    }
}