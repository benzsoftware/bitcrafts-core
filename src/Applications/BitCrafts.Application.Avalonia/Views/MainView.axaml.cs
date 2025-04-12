using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Avalonia.Controls.Views;

namespace BitCrafts.Application.Avalonia.Views;

public partial class MainView : BaseView, IMainView
{
    public MainView()
    {
        InitializeComponent();
    }

    public event EventHandler CloseEvent;

    public void SetBusy(string message)
    {
        LoadingControl.IsVisible = true;
        LoadingText.Text = message;
        RootDockPanel.IsVisible = false;
    }

    public void UnsetBusy()
    {
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


    public void ShowError(string message)
    {
        ErrorTextBox.Text = message;
    }


    private void QuitMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        CloseEvent?.Invoke(this, EventArgs.Empty);
    }

    protected override IModel UpdateModelFromInputsCore()
    {
        return Model;
    }
}