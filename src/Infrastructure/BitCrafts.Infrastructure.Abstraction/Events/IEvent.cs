namespace BitCrafts.Infrastructure.Abstraction.Events;

/// <summary>
///     Defines the base interface for all events in the application.
///     Events are used for loosely coupled communication between different parts of the application.
/// </summary>
public interface IEvent
{
    /// <summary>
    ///     Gets the timestamp of when the event occurred.
    /// </summary>
    DateTimeOffset Timestamp { get; }
}