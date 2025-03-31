using Avalonia.Controls;
using BitCrafts.Infrastructure.Abstraction.Application.Views;

namespace BitCrafts.Infrastructure.Application.Views;

public interface IMainView : IView
{
    event EventHandler CloseEvent;
    Menu GetMenuControl();
    TabControl GetTabControl();
}