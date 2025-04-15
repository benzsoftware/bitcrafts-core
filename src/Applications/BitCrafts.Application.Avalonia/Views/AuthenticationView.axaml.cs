using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Events;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Controls.Loading;
using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Infrastructure.Abstraction.Extensions;
using BitCrafts.Infrastructure.Abstraction.Services;

namespace BitCrafts.Application.Avalonia.Views;

public partial class AuthenticationView : BaseView, IAuthenticationView
{
    public AuthenticationView()
    {
        InitializeComponent();
    }

    public void SetAuthenticationError(string errorMessage)
    {
        ErrorMessgeTextBlox.Text = errorMessage;
    }

    private void AuthenticateButton_OnClick(object sender, RoutedEventArgs e)
    {
        EventAggregator.Publish(ViewEvents.Authentication.AuthenticateEventName);
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        EventAggregator.Publish(ViewEvents.Base.CloseWindowEventName);
    }

    private void EnvironmentButton_OnClick(object sender, RoutedEventArgs e)
    {
        EventAggregator.Publish(ViewEvents.Authentication.ShowEnvironmentsEventName);
    }

    protected override IModel UpdateModelFromInputsCore()
    {
        return new AuthenticationModel(LoginTextBox.Text.TrimOrEmpty(), PasswordTextBox.Text.TrimOrEmpty(),
            EnvironmentComboBox.SelectedItem as EnvironmentConfiguration);
    }

    protected override void ClearCore()
    {
        LoginTextBox.Text = string.Empty;
        PasswordTextBox.Text = string.Empty;
        EnvironmentComboBox.SelectedIndex = -1;
    }

    protected override LoadingControl LoadingIndicator => BusyControl;
}