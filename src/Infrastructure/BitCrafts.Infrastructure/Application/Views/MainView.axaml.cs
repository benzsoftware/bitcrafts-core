using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Infrastructure.Abstraction.Modules;
using BitCrafts.Infrastructure.Application.Events;
using BitCrafts.Infrastructure.Avalonia.Views;

namespace BitCrafts.Infrastructure.Application.Views;

public partial class MainView : BaseControl, IMainView
{
    private IReadOnlyList<IModule> _modules;

    public MainView()
    {
        InitializeComponent();
    }

    public event EventHandler CloseEvent;
    public event EventHandler<MenuClickEventArgs> MenuClickEvent;

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

    public void SetupMenu(IReadOnlyList<IModule> modules)
    {
        _modules = modules;
        ModuleMenuItem.Items.Clear();
        foreach (var x in modules)
        {
            var menuItem = new MenuItem();
            menuItem.Header = x.Name;
            menuItem.DataContext = x;
            menuItem.Click += MenuItemOnClick;
            ModuleMenuItem.Items.Add(menuItem);
        }
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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var item in ModuleMenuItem.Items)
            {
                var tabItem = item as MenuItem;
                if (tabItem != null) tabItem.Click -= MenuItemOnClick;
            }

            ModuleMenuItem.Items.Clear();
            _modules = null;
        }

        base.Dispose(disposing);
    }

    private void MenuItemOnClick(object sender, RoutedEventArgs e)
    {
        e.Handled = true;
        var menuItem = (MenuItem)sender;
        MenuClickEvent?.Invoke(this, new MenuClickEventArgs
        {
            Module = menuItem.DataContext as IModule
        });
    }


    protected override void OnAppeared()
    {
    }


    private void QuitMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        CloseEvent?.Invoke(this, EventArgs.Empty);
    }
}