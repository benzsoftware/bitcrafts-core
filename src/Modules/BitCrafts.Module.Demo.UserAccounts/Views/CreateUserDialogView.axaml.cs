using Avalonia.Interactivity;
using BitCrafts.Infrastructure.Avalonia.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Views;

namespace BitCrafts.Module.Demo.UserAccounts.Views;

public partial class CreateUserDialogView : BaseControl, ICreateUserDialogView
{
    public CreateUserDialogView()
    {
        InitializeComponent();
    }

    public event EventHandler<User> UserCreated;


    protected override void OnAppeared()
    {
    }

    protected override void OnDisappeared()
    {
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        UserCreated?.Invoke(this, GetUserEntity());
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        ClearFields();
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
            FirstName = FirstNameTextBox.Text,
            LastName = LastNameTextBox.Text,
            Email = EmailTextBox.Text,
            Password = PasswordTextBox.Text,
            IsActive = IsActiveCheckBox.IsChecked ?? false // Handle nullable bool
        };
        return newUser;
    }
}