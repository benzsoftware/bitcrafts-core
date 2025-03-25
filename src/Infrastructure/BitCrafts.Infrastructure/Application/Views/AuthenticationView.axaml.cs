using Avalonia.Interactivity;
using BitCrafts.Infrastructure.Abstraction.Application.Views;
using BitCrafts.Infrastructure.Abstraction.Data;
using BitCrafts.Infrastructure.Avalonia.Views;

namespace BitCrafts.Infrastructure.Application.Views;

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
}