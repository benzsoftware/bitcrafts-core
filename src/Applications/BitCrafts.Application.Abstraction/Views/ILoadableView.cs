using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Presenters;

namespace BitCrafts.Application.Abstraction.Views;

public interface ILoadableView : IView
{
    void ShowLoadingMessage(string message);
}