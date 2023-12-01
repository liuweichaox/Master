using Master.Domain.SeedWork;

namespace Master.Domain.Rules;

public class CustomerEmailMustBeUniqueRule:IBusinessRule
{
    private readonly string _email;

    public CustomerEmailMustBeUniqueRule(string email)
    {
        _email = email;
    }

    public bool IsBroken() => string.IsNullOrEmpty(_email);

    public string Message => "Customer email must be unique";
}