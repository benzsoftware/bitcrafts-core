using System.ComponentModel.DataAnnotations;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Abstraction.Views;
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
        UpdateModelFromInputsCore();
        EventAggregator.Publish<AuthenticationModel>(IAuthenticationView.AuthenticateEventName,
            (AuthenticationModel)Model);
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

    protected override IModel UpdateModelFromInputsCore()
    {
        return new AuthenticationModel(LoginTextBox.Text.TrimOrEmpty(), PasswordTextBox.Text.TrimOrEmpty(),
            EnvironmentComboBox.SelectedItem as EnvironmentConfiguration);
    }

    protected override void ClearCore()
    {
    }

    protected override bool ValidateModelCore(out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        return true;
    }
 
}