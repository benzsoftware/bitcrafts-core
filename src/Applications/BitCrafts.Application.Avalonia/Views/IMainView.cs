using Avalonia.Controls;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Views;

public interface IMainView : IView
{
    event EventHandler CloseEvent;
    Menu GetMenuControl();
    TabControl GetTabControl();
}