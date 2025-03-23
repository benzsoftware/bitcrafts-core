using System.Collections.ObjectModel;
using Avalonia.Input;
using Avalonia.Interactivity;
using BitCrafts.Infrastructure.Avalonia.Views;
using BitCrafts.Module.Finance.Abstraction.Data;
using BitCrafts.Module.Finance.Abstraction.Views;

namespace BitCrafts.Module.Finance.Views;

public partial class DisplayBankAccountsView : BaseControl, IDisplayBankAccountsView
{
    public DisplayBankAccountsView()
    {
        InitializeComponent();
    }

    private ObservableCollection<BankAccount> BankAccounts { get; } = new();
    public event EventHandler<BankAccount> DeleteBankAccount;

    public void RefreshBankAccounts(IEnumerable<BankAccount> bankAccounts)
    {
        BankAccounts.Clear();
        foreach (var bankAccount in bankAccounts) BankAccounts.Add(bankAccount);
    }

    private void BankAccountsDataGrid_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Delete) DeleteBankAccount?.Invoke(this, BankAccountsDataGrid.SelectedItem as BankAccount);
    }

    protected override void OnAppeared()
    {
        BankAccountsDataGrid.ItemsSource = BankAccounts;
    }

    protected override void OnDisappeared()
    {
    }

    private void CreateUserButton_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void UpdateUserButton_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void DeleteUserButton_OnClick(object sender, RoutedEventArgs e)
    {
    }
}