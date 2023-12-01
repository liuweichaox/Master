namespace Master.Infrastructure.Processing
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}