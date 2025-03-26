using Avalonia.Controls;
using BitCrafts.Infrastructure.Abstraction.Application.Views;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Infrastructure.Application.Events;

namespace BitCrafts.Infrastructure.Application.Views;

public interface IMainView : IView
{
    event EventHandler CloseEvent;
    Menu GetMenuControl();
    TabControl GetTabControl();
}