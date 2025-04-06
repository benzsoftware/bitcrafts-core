using Avalonia.Controls;
using Avalonia.Interactivity;
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

    protected override void OnDisappeared()
    {
    }

    public override void ShowError(string message)
    {
        ErrorTextBox.Text = message;
    }

    protected override void OnAppeared()
    {
    }


    private void QuitMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        CloseEvent?.Invoke(this, EventArgs.Empty);
    }
}