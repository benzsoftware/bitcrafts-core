using BitCrafts.Infrastructure.Abstraction.Events;

namespace BitCrafts.Application.Abstraction.Views;

public interface IEventAware
{
    void SetEventAggregator(IEventAggregator eventAggregator);
}