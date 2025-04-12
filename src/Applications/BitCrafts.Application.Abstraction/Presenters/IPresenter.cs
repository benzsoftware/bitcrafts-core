using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Abstraction.Presenters;

public interface IPresenter : IDisposable
{
    IView View { get; }
    void SetView(IView view);
    void SetParameters(Dictionary<string, object> parameters);
}