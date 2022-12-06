using Velen.Domain.SeedWork;
using Velen.Infrastructure.Commands;

namespace Velen.Application.Customers
{
    public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, CustomerDto>
    {
        public async Task<CustomerDto> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var guid=Guid.NewGuid();
            return await  Task.Run(() =>
            {
                return new CustomerDto()
                {
                    Id = guid
                };
            });
        }
    }
}