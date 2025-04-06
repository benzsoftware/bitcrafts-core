using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using BitCrafts.Application.Avalonia.Controls.Views;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.Views;
using BitCrafts.Modules.Demo.UserAccounts.Abstraction.Data;

namespace BitCrafts.Module.Finance.Views;

public partial class CreateBankAccountView : BaseView, ICreateBankAccountView
{
    public CreateBankAccountView()
    {
        InitializeComponent();
    }

    private ObservableCollection<User> Users { get; } = new();
    private User SelectedUser { get; set; }

    public void ClearFields()
    {
        AccountNameTextBox.Text = string.Empty;
        AccountNumberTextBox.Text = string.Empty;
        AccountInitialBalanceTextBox.Text = string.Empty;
    }

    public void RefreshUsers(IEnumerable<User> users)
    {
        Users.Clear();
        foreach (var user in users) Users.Add(user);
    }

    public event EventHandler<BankAccount> CreateBankAccount;
    public event EventHandler CloseView;

    private void UsersComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            var eAddedItem = e.AddedItems[0] as User;
            SelectedUser = eAddedItem;
        }
    }

    private void AddAccountButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ValidateBankAccount())
        {
            CreateBankAccount?.Invoke(this, GetBankAccount());
            ClearFields();
        }
    }

    private BankAccount GetBankAccount()
    {
        return new BankAccount
        {
            AccountName = AccountNameTextBox.Text.Trim(),
            AccountNumber = AccountNumberTextBox.Text.Trim(),
            Balance = decimal.Parse(AccountInitialBalanceTextBox.Text.Trim()),
            UserId = SelectedUser.Id
        };
    }

    private bool ValidateBankAccount()
    {
        return !string.IsNullOrWhiteSpace(AccountNameTextBox.Text.Trim()) &&
               !string.IsNullOrWhiteSpace(AccountNumberTextBox.Text.Trim()) &&
               decimal.TryParse(AccountInitialBalanceTextBox.Text.Trim(), out _);
    }

    public override void ShowError(string message)
    {
    }

    protected override void OnAppeared()
    {
        UsersComboBox.ItemsSource = Users;
    }

    protected override void OnDisappeared()
    {
        Users.Clear();
    }

    private void CloseViewButton_OnClick(object sender, RoutedEventArgs e)
    {
        CloseView?.Invoke(this, EventArgs.Empty);
    }
}