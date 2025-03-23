namespace BitCrafts.Infrastructure.Abstraction.Events;

/// <summary>
///     Defines an interface for an event aggregator.
///     The event aggregator facilitates communication between objects
///     by allowing them to publish events and subscribe to events
///     without knowing about each other directly.
/// </summary>
public interface IEventAggregator
{
    /// <summary>
    ///     Subscribes a handler to an event of type TEvent.
    ///     The handler will be invoked when an event of type TEvent is published.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to subscribe to.</typeparam>
    /// <param name="handler">The action to take when the event is published.</param>
    void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;

    /// <summary>
    ///     Unsubscribes a handler from an event of type TEvent.
    ///     The handler will no longer be invoked when an event of type TEvent is published.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to unsubscribe from.</typeparam>
    /// <param name="handler">The action to remove from the subscribers list.</param>
    void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IEvent;

    /// <summary>
    ///     Publishes an event to all subscribers.
    ///     Each subscriber's handler will be invoked with the provided event instance.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to publish.</typeparam>
    /// <param name="request">The event instance to publish.</param>
    void Publish<TEvent>(TEvent request) where TEvent : IEvent;
}