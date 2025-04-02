namespace BitCrafts.Infrastructure.Abstraction.Events;

public interface IEventAggregator
{
    Guid Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;

    bool Unsubscribe<TEvent>(Guid handlerId) where TEvent : IEvent;

    void Publish<TEvent>(TEvent eventItem) where TEvent : IEvent;
}