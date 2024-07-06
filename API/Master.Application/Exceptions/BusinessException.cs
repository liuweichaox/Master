namespace Master.Application.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string Details { get; }
}