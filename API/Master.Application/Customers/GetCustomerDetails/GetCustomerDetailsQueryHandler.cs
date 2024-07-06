using Dapper;
using Master.Domain.Data;
using Master.Infrastructure.Queries;

namespace Master.Application.Customers.GetCustomerDetails;

public class GetCustomerDetailsQueryHandler : IQueryHandler<GetCustomerDetailsQuery, CustomerDetailsDTO>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetCustomerDetailsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public Task<CustomerDetailsDTO> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, Name, Email, WelcomeEmailWasSent FROM Customers WHERE Id= @CustomerId ";

        var connection = _sqlConnectionFactory.GetOpenConnection();

        return connection.QuerySingleAsync<CustomerDetailsDTO>(sql, new
        {
            request.CustomerId
        });
    }
}