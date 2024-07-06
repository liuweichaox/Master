using System.Text.Json.Serialization;
using Master.Domain.SeedWork;

namespace Master.Domain.Events;

public class DomainNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
{
    public DomainNotificationBase(T domainEvent)
    {
        Id = Guid.NewGuid();
        DomainEvent = domainEvent;
    }

    [JsonIgnore] public T DomainEvent { get; }

    public Guid Id { get; }
}