namespace Master.Application.DTOs;

public class CustomerDetailsDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string WelcomeEmailWasSent { get; set; }
}