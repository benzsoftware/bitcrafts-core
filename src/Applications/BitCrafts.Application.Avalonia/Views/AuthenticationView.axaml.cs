using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Views;
using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Infrastructure.Abstraction.Data;

namespace BitCrafts.Application.Avalonia.Views;

public partial class AuthenticationView : BaseControl, IAuthenticationView
{
    public AuthenticationView()
    {
        InitializeComponent();
    }


    protected override void OnAppeared()
    {
    }

    protected override void OnDisappeared()
    {
    }

    private Authentication GetAuthentication()
    {
        return new Authentication()
        {
            Login = LoginTextBox.Text?.Trim(),
            Password = PasswordTextBox.Text?.Trim()
        };
    }

    public event EventHandler Cancel;
    public event EventHandler<Authentication> Authenticate;

    public void SetAuthenticationError(string errorMessage)
    {
        ErrorMessgeTextBlox.Text = errorMessage;
    }

    private void AuthenticateButton_OnClick(object sender, RoutedEventArgs e)
    {
        Authenticate?.Invoke(this, GetAuthentication());
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Cancel?.Invoke(this, EventArgs.Empty);
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
}