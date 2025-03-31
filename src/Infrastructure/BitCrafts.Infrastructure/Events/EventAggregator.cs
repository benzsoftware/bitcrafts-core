using System.Collections.Concurrent;
using BitCrafts.Infrastructure.Abstraction.Events;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Events;

public sealed class EventAggregator : IEventAggregator
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, Delegate>> _handlers = new();
    private readonly ILogger<EventAggregator> _logger;
    private bool _isDisposed;

    public EventAggregator(ILogger<EventAggregator> logger)
    {
        _logger = logger;
    }


    public Guid Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        var handlerId = Guid.NewGuid();

        var handlers = _handlers.GetOrAdd(eventType, _ => new ConcurrentDictionary<Guid, Delegate>());
        handlers[handlerId] = handler;

        _logger.LogDebug("Handler for {EventType} subscribed with ID {HandlerId}", eventType.Name, handlerId);

        return handlerId;
    }


    public bool Unsubscribe<TEvent>(Guid handlerId) where TEvent : IEvent
    {
        var eventType = typeof(TEvent);

        if (_handlers.TryGetValue(eventType, out var handlers))
        {
            var result = handlers.TryRemove(handlerId, out _);

            if (result)
            {
                _logger.LogDebug("Handler for {EventType} with ID {HandlerId} unsubscribed", eventType.Name, handlerId);

                if (handlers.IsEmpty) _handlers.TryRemove(eventType, out _);
            }

            return result;
        }

        return false;
    }


    public void Publish<TEvent>(TEvent eventItem) where TEvent : IEvent
    {
        if (_isDisposed)
        {
            _logger.LogWarning("Event {EventType} publication ignored: EventAggregator is disposed",
                typeof(TEvent).Name);
            return;
        }

        if (!_handlers.TryGetValue(typeof(TEvent), out var handlers) || handlers.IsEmpty)
        {
            return;
        }

        var currentHandlers = handlers.ToArray();

        _logger.LogDebug("Publishing event {EventType} to {HandlerCount} handlers",
            typeof(TEvent).Name, currentHandlers.Length);

        foreach (var handler in currentHandlers)
            try
            {
                if (handler.Value is Action<TEvent> action) action(eventItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in event handler for {EventType}", typeof(TEvent).Name);
            }
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _handlers.Clear();
        _isDisposed = true;
    }
}