using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Abstraction.Presenters;

public interface IPresenter : IDisposable
{
    IView View { get; }
    void SetParameters(Dictionary<string, object> parameters);
}