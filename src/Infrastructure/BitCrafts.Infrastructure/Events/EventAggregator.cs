using System.Collections.Concurrent;
using BitCrafts.Infrastructure.Abstraction.Events;
using Microsoft.Extensions.Logging;

namespace BitCrafts.Infrastructure.Events;

public sealed class EventAggregator : IEventAggregator, IDisposable
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, Delegate>> _handlers = new();
    private readonly ILogger<EventAggregator> _logger;
    private readonly ConcurrentBag<WeakReference<Subscription>> _subscriptions;

    private bool _isDisposed;

    public EventAggregator(ILogger<EventAggregator> logger)
    {
        _logger = logger;
        _subscriptions = new ConcurrentBag<WeakReference<Subscription>>();
    }

    public IDisposable Subscribe(string eventKey, Action handler)
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(EventAggregator));

        if (handler == null)
            throw new ArgumentNullException(nameof(handler));


        if (string.IsNullOrWhiteSpace(eventKey))
            throw new ArgumentException("Event key cannot be null or empty", nameof(eventKey));

        var subscriptionId = Guid.NewGuid();
        var handlers = _handlers.GetOrAdd(eventKey, _ => new ConcurrentDictionary<Guid, Delegate>());
        handlers[subscriptionId] = handler;

        _logger.LogDebug("Handler for event '{EventKey}' subscribed with ID {SubscriptionId}",
            eventKey, subscriptionId);

        var subscription = new Subscription(this, eventKey, subscriptionId);
        _subscriptions.Add(new WeakReference<Subscription>(subscription));
        return subscription;
    }

    public IDisposable Subscribe<TPayload>(string eventKey, Action<TPayload> handler)
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(EventAggregator));

        if (string.IsNullOrWhiteSpace(eventKey))
            throw new ArgumentException("Event key cannot be null or empty", nameof(eventKey));

        if (handler == null)
            throw new ArgumentNullException(nameof(handler));

        var subscriptionId = Guid.NewGuid();
        var handlers = _handlers.GetOrAdd(eventKey, _ => new ConcurrentDictionary<Guid, Delegate>());
        handlers[subscriptionId] = handler;

        _logger.LogInformation("Handler for event '{EventKey}' subscribed with ID {SubscriptionId}",
            eventKey, subscriptionId);

        return new Subscription(this, eventKey, subscriptionId);
    }

    public void Publish(string eventKey)
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(EventAggregator));

        if (string.IsNullOrWhiteSpace(eventKey))
            throw new ArgumentException("Event key cannot be null or empty", nameof(eventKey));

        if (!_handlers.TryGetValue(eventKey, out var handlers) || handlers.IsEmpty)
            return;

        var currentHandlers = handlers.ToArray();

        _logger.LogDebug("Publishing event '{EventKey}' to {HandlerCount} handlers",
            eventKey, currentHandlers.Length);

        foreach (var handler in currentHandlers)
            try
            {
                if (handler.Value is Action action) action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in event handler for event '{EventKey}'", eventKey);
            }
    }

    public void Publish<TPayload>(string eventKey, TPayload payload)
    {
        if (_isDisposed)
            throw new ObjectDisposedException(nameof(EventAggregator));

        if (string.IsNullOrWhiteSpace(eventKey))
            throw new ArgumentException("Event key cannot be null or empty", nameof(eventKey));

        if (payload == null)
            throw new ArgumentNullException(nameof(payload));

        if (!_handlers.TryGetValue(eventKey, out var handlers) || handlers.IsEmpty)
            return;

        var currentHandlers = handlers.ToArray();

        _logger.LogDebug("Publishing event '{EventKey}' with payload to {HandlerCount} handlers",
            eventKey, currentHandlers.Length);

        foreach (var handler in currentHandlers)
        {
            try
            {
                // Essayer d'abord avec le type exact pour le payload
                if (handler.Value is Action<TPayload> typedAction)
                    typedAction(payload);
                // Si c'est juste une Action, l'appeler sans paramètre
                else if (handler.Value is Action action) action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in event handler for event '{EventKey}'", eventKey);
            }
        }
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        foreach (var weakRef in _subscriptions)
            if (weakRef.TryGetTarget(out var subscription))
                subscription.Dispose();

        _handlers.Clear();
        _subscriptions.Clear();
        _isDisposed = true;
    }

    internal bool Unsubscribe(string eventKey, Guid subscriptionId)
    {
        if (string.IsNullOrWhiteSpace(eventKey))
            throw new ArgumentException("Event key cannot be null or empty", nameof(eventKey));

        if (_handlers.TryGetValue(eventKey, out var handlers))
        {
            var result = handlers.TryRemove(subscriptionId, out _);

            if (result)
            {
                _logger.LogDebug("Handler for event '{EventKey}' with ID {SubscriptionId} unsubscribed",
                    eventKey, subscriptionId);

                if (handlers.IsEmpty)
                    _handlers.TryRemove(eventKey, out _);
            }

            return result;
        }

        return false;
    }
}