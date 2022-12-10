using Velen.Infrastructure.Queries;

namespace Velen.Application.Customers.GetCustomerDetails
{
    public class GetCustomerDetailsQuery : IQuery<CustomerDetailsDto>
    {
        public GetCustomerDetailsQuery(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}