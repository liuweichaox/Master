using Master.Application.Commands;
using Master.Application.DTOs;
using Master.Domain.Customers;
using Master.Domain.IRepositories;
using Master.Domain.SeedWork;
using Master.Infrastructure.Commands;

namespace Master.Application.CommandHandles;

public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, CustomerDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDTO> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.CreateRegistered(request.Email, request.Name);

        await _customerRepository.AddAsync(customer);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new CustomerDTO { Id = customer.Id };
    }
}