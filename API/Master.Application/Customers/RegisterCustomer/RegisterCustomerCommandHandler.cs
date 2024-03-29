﻿using Master.Domain.Customers;
using Master.Domain.IRepositories;
using Master.Domain.SeedWork;
using Master.Infrastructure.Commands;

namespace Master.Application.Customers.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCustomerCommandHandler(
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            this._customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerDto> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = Customer.CreateRegistered(request.Email, request.Name);

            await this._customerRepository.AddAsync(customer);

            await this._unitOfWork.CommitAsync(cancellationToken);

            return new CustomerDto { Id = customer.Id };
        }
    }
}