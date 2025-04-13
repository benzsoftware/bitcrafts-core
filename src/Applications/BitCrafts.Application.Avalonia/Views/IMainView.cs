using Avalonia.Controls;
using BitCrafts.Application.Abstraction.Views;

namespace BitCrafts.Application.Avalonia.Views;

public interface IMainView : IView
{
    Menu GetMenuControl();
    TabControl GetTabControl();
}