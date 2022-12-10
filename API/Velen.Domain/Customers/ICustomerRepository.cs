namespace Velen.Domain.Customers
{
    public interface ICustomerRepository
    {
        ValueTask<Customer?> GetByIdAsync(Guid id);

        ValueTask AddAsync(Customer customer);
    }
}