namespace Master.Application.Exceptions;

public class InvalidCommandException : Exception
{
    public InvalidCommandException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string Details { get; }
}