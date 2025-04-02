using Avalonia.Controls;
using BitCrafts.Application.Abstraction.Application.Views;

namespace BitCrafts.Application.Avalonia.Views;

public interface IMainView : IView
{
    event EventHandler CloseEvent;
    Menu GetMenuControl();
    TabControl GetTabControl();
    void SetBusy(string message);
    void UnsetBusy();
}