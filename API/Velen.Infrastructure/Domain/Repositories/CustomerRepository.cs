using Velen.Domain.Customers;
using Velen.Domain.IRepositories;

namespace Velen.Infrastructure.Domain.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public ValueTask<Customer?> GetByIdAsync(Guid id)
    {
        return _context.Customers.FindAsync(id);
    }

    public async ValueTask AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
    }
}