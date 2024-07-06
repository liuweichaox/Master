using Master.Infrastructure.Queries;

namespace Master.Application.Customers.GetCustomerDetails;

public class GetCustomerDetailsQuery : IQuery<CustomerDetailsDTO>
{
    public GetCustomerDetailsQuery(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; }
}