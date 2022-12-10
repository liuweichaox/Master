using System.Text.Json.Serialization;
using MediatR;
using Velen.Infrastructure.Commands;

namespace Velen.Application.Customers.IntegrationHandlers
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