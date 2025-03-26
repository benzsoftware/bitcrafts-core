using Avalonia.Controls;
using BitCrafts.Infrastructure.Abstraction.Application.Views;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Infrastructure.Application.Events;

namespace BitCrafts.Infrastructure.Application.Views;

public interface IMainView : IView
{
    event EventHandler CloseEvent;
    event EventHandler<MenuClickEventArgs> MenuClickEvent;
    void SetupMenu(IReadOnlyList<IModule> modules);
    Menu GetMenuControl();
    TabControl GetTabControl();
}