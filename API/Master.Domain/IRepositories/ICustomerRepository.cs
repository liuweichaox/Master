using Master.Domain.Customers;

namespace Master.Domain.IRepositories;

public interface ICustomerRepository
{
    ValueTask<Customer?> GetByIdAsync(Guid id);

    ValueTask AddAsync(Customer customer);
}