using Avalonia.Controls;
using Avalonia.Interactivity;
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

    public event EventHandler CloseEvent;


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
        CloseEvent?.Invoke(this, EventArgs.Empty);
    }

    protected override IModel UpdateModelFromInputsCore()
    {
        return Model;
    }

    protected override LoadingControl LoadingIndicator => BusyControl;
}