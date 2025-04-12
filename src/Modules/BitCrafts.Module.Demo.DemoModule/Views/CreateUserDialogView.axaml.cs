using Avalonia.Interactivity;
using BitCrafts.Application.Abstraction.Models;
using BitCrafts.Application.Avalonia.Controls.Loading;
using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;

namespace BitCrafts.Module.Demo.DemoModule.Views;

public partial class CreateUserDialogView : BaseView, ICreateUserDialogView
{
    public CreateUserDialogView()
    {
        InitializeComponent();
    }

    public event EventHandler<User> UserCreated;

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (AreFielsValid())
            UserCreated?.Invoke(this, GetUserEntity());
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClearFields();
    }

    private bool AreFielsValid()
    {
        return !string.IsNullOrWhiteSpace(FirstNameTextBox.Text.Trim()) &&
               !string.IsNullOrWhiteSpace(LastNameTextBox.Text.Trim()) &&
               !string.IsNullOrWhiteSpace(EmailTextBox.Text.Trim()) &&
               !string.IsNullOrWhiteSpace(PasswordTextBox.Text.Trim());
    }

    private void ClearFields()
    {
        FirstNameTextBox.Text = string.Empty;
        LastNameTextBox.Text = string.Empty;
        EmailTextBox.Text = string.Empty;
        PasswordTextBox.Text = string.Empty;
        IsActiveCheckBox.IsChecked = false;
    }

    private User GetUserEntity()
    {
        var newUser = new User
        {
            FirstName = FirstNameTextBox.Text.Trim(),
            LastName = LastNameTextBox.Text.Trim(),
            Email = EmailTextBox.Text.Trim(),
            Password = PasswordTextBox.Text.Trim(),
            IsActive = IsActiveCheckBox.IsChecked ?? false
        };
        return newUser;
    }

    protected override IModel UpdateModelFromInputsCore()
    {
        return Model;
    }

    protected override LoadingControl LoadingIndicator => null;
}