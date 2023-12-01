namespace Master.Infrastructure
{
    public interface IExecutionContextAccessor
    {
        Guid CorrelationId { get; }

        bool IsAvailable { get; }
    }
}
