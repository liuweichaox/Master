using Master.Application.DTOs;
using Master.Infrastructure.Queries;

namespace Master.Application.Queries;

public class GetCustomerDetailsQuery : IQuery<CustomerDetailsDTO>
{
    public GetCustomerDetailsQuery(Guid customerId)
    {
        CustomerId = customerId;
    }

    public Guid CustomerId { get; }
}