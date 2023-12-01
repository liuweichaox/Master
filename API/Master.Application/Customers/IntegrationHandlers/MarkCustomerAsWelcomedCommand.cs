using System.Text.Json.Serialization;
using MediatR;
using Master.Infrastructure.Commands;

namespace Master.Application.Customers.IntegrationHandlers
{
    public class MarkCustomerAsWelcomedCommand : InternalCommandBase<Unit>
    {
        [JsonConstructor]
        public MarkCustomerAsWelcomedCommand(Guid id, Guid customerId) : base(id)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}