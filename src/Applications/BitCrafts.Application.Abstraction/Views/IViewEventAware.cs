using BitCrafts.Infrastructure.Abstraction.Events;

namespace BitCrafts.Application.Abstraction.Views;

public interface IViewEventAware
{
    void SetEventAggregator(IEventAggregator eventAggregator);
}