namespace BitCrafts.Infrastructure.Abstraction.Events;

public interface IEventAggregator
{
    IDisposable Subscribe(string eventKey, Action handler);
    IDisposable Subscribe<TPayload>(string eventKey, Action<TPayload> handler);
    void Publish(string eventKey);
    void Publish<TPayload>(string eventKey, TPayload payload);
}