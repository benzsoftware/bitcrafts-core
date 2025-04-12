using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Abstraction.Presenters;

public interface IPresenter : IDisposable
{
    IView View { get; }
    void SetView(Type viewType);
    void SetParameters(Dictionary<string, object> parameters);
}