using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Avalonia.Controls.Loading;
using BitCrafts.Application.Avalonia.Controls.Views;

namespace BitCrafts.Application.Avalonia.Views;

public partial class MainView : BaseView, IMainView
{
    public MainView()
    {
        InitializeComponent();
    }
 
    public Menu GetMenuControl()
    {
        return MainMenu;
    }

    public TabControl GetTabControl()
    {
        return MainTabControl;
    }
    
    private void QuitMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        EventAggregator.Publish(ViewEvents.Base.CloseWindowEventName);
    }

    protected override LoadingControl LoadingIndicator => BusyControl;
}