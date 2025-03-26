using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Infrastructure.Application.Events;
using BitCrafts.Infrastructure.Avalonia.Views;

namespace BitCrafts.Infrastructure.Application.Views;

public partial class MainView : BaseControl, IMainView
{
    public MainView()
    {
        InitializeComponent();
    }

    public event EventHandler CloseEvent;

    public override void SetBusy(string message)
    {
        base.SetBusy(message);
        LoadingControl.IsVisible = true;
        LoadingText.Text = BusyText;
        RootDockPanel.IsVisible = false;
    }

    public override void UnsetBusy()
    {
        base.UnsetBusy();
        LoadingControl.IsVisible = false;
        LoadingText.Text = string.Empty;
        RootDockPanel.IsVisible = true;
    }
 

    public Menu GetMenuControl()
    {
        return MainMenu;
    }

    public TabControl GetTabControl()
    {
        return MainTabControl;
    }

    protected override void OnDisappeared()
    {
    }
    protected override void OnAppeared()
    {
    }


    private void QuitMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        CloseEvent?.Invoke(this, EventArgs.Empty);
    }
}