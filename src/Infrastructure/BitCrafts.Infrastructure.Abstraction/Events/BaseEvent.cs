namespace BitCrafts.Infrastructure.Abstraction.Events;

/// <summary>
///     Provides a base class for events, implementing the IEvent interface
///     and adding a default timestamp.
/// </summary>
public abstract class BaseEvent : IEvent
{
    /// <summary>
    ///     Gets or sets the timestamp of when the event occurred.
    ///     Defaults to the current date and time when the event is created.
    /// </summary>
    public DateTimeOffset Timestamp { get; protected set; } = DateTimeOffset.Now;
}