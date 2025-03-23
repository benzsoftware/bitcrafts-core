using System.Collections.Concurrent;
using BitCrafts.Infrastructure.Abstraction.Events;

namespace BitCrafts.Infrastructure.Events;

public sealed class EventAggregator : IEventAggregator
{
    private readonly ConcurrentDictionary<Type, List<EventHandlerWrapper>> _handlers = new();
    private readonly object _lock = new();

    public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        lock (_lock)
        {
            if (!_handlers.ContainsKey(typeof(TEvent))) _handlers[typeof(TEvent)] = new List<EventHandlerWrapper>();

            _handlers[typeof(TEvent)].Add(new EventHandlerWrapper(e => handler((TEvent)e)));
        }
    }

    public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        lock (_lock)
        {
            if (_handlers.ContainsKey(typeof(TEvent)))
            {
                var handlerToRemove = _handlers[typeof(TEvent)].FirstOrDefault(h =>
                    h.Handler == (Action<object>)(e => handler((TEvent)e)));

                if (handlerToRemove != null) _handlers[typeof(TEvent)].Remove(handlerToRemove);
            }
        }
    }

    public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
    {
        List<EventHandlerWrapper> handlers;
        lock (_lock)
        {
            if (!_handlers.ContainsKey(typeof(TEvent))) return;

            handlers = _handlers[typeof(TEvent)].ToList();
        }

        foreach (var handler in handlers) handler.Handler(@event);
    }

    private class EventHandlerWrapper
    {
        public EventHandlerWrapper(Action<object> handler)
        {
            Handler = handler;
        }

        public Action<object> Handler { get; }

        public override bool Equals(object obj)
        {
            if (obj is EventHandlerWrapper other) return Handler == other.Handler;

            return false;
        }

        public override int GetHashCode()
        {
            return Handler.GetHashCode();
        }
    }
}