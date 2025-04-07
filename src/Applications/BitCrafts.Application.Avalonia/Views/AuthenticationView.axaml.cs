using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Controls.Views;

namespace BitCrafts.Application.Avalonia.Views;

public partial class AuthenticationView : LoadableView<AuthenticationViewModel>, IAuthenticationView
{
    public AuthenticationView()
    {
        InitializeComponent();
    }


    protected override void OnDataDisplayed(AuthenticationViewModel model)
    {
    }

    protected override Control LoadingIndicator => null;

    protected override TextBlock ErrorTextBlock => null;

    protected override void SetModel()
    {
        Model = new AuthenticationViewModel(LoginTextBox?.Text?.Trim(), PasswordTextBox?.Text?.Trim(), null);
    }

    public void SetAuthenticationError(string errorMessage)
    {
        ErrorMessgeTextBlox.Text = errorMessage;
    }

    private void AuthenticateButton_OnClick(object sender, RoutedEventArgs e)
    {
        SetModel();
        EventAggregator.Publish<AuthenticationViewModel>(IAuthenticationView.AuthenticateEventName, Model);
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        EventAggregator.Publish(IAuthenticationView.CancelEventName);
    }

    public void DisplayProgressBar()
    {
        AuthenticatingProgressBar.IsVisible = true;
        AuthenticateButton.IsEnabled = false;
        CancelButton.IsEnabled = false;
    }

    public void HideProgressBar()
    {
        AuthenticatingProgressBar.IsVisible = false;
        AuthenticateButton.IsEnabled = true;
        CancelButton.IsEnabled = true;
    }

    private void EnvironmentButton_OnClick(object sender, RoutedEventArgs e)
    {
        EventAggregator.Publish(IAuthenticationView.ShowEnvironmentEventName);
    }
}