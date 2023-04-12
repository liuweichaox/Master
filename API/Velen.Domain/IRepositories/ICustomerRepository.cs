using Velen.Domain.Customers;

namespace Velen.Domain.IRepositories
{
    public interface ICustomerRepository
    {
        ValueTask<Customer?> GetByIdAsync(Guid id);

        ValueTask AddAsync(Customer customer);
    }
}