namespace BitCrafts.Infrastructure.Events;

internal class Subscription : IDisposable
{
    private readonly EventAggregator _aggregator;
    private readonly string _eventKey;
    private readonly Guid _subscriptionId;
    private bool _disposed;

    public Subscription(EventAggregator aggregator, string eventKey, Guid subscriptionId)
    {
        _aggregator = aggregator ?? throw new ArgumentNullException(nameof(aggregator));
        if (string.IsNullOrWhiteSpace(eventKey))
            throw new ArgumentException("Event key cannot be null or empty", nameof(eventKey));

        _eventKey = eventKey;
        _subscriptionId = subscriptionId;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _aggregator.Unsubscribe(_eventKey, _subscriptionId);
            _disposed = true;
        }
    }
}